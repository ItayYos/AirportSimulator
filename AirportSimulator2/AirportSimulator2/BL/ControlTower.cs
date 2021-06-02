using AirportSimulator2.AirportLayoutBuilder;
using AirportSimulator2.LogicalEntities;
using AirportModels;
using AirportSimulator2.Services;
using AirportSimulator2.WS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace AirportSimulator2.BL
{
    /* Monitor flights and manage flights requests. */
    public class ControlTower : IControlTower
    {
        private Collection<Leg> _airportLayout;
        private Queue<IFlight> _requestQueue;
        private bool _started;
        private System.Timers.Timer _timer;
        private IWebSocketHandler _webSockets;
        private List<IFlight> _activeFlights;
        private IAirportLog _logger;

        public ControlTower(IAirportBuilder airportBuilder, IWebSocketHandler webSockets, IAirportLog logger)
        {
            _airportLayout = new Collection<Leg>(airportBuilder.Build(2));
            _requestQueue = new Queue<IFlight>();
            _started = false;
            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += TimerInterval;
            _webSockets = webSockets;
            _activeFlights = new List<IFlight>();
            _logger = logger;
        }
        private void Register(IFlight flight)
        {
            /* Add flight to active flights and notify any ws clients. 
             * Does nothing if flight already exists in active flights list. */
            try
            {
                lock (_activeFlights)
                {
                    if (!_activeFlights.Contains(flight))
                        _activeFlights.Add(flight);
                    UpdateClientsFlightAsync(flight, null);
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private void UnRegister(IFlight flight)
        {
            /* Removes given flight from active flights list. */
            try
            {
                lock (_activeFlights)
                {
                    if (_activeFlights.Contains(flight))
                        _activeFlights.Remove(flight);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        private async Task UpdateClientsFlightAsync(IFlight flight, Leg nextLeg)
        {
            /* Prepare report and notifies all ws active connections. */
            WSClientReport report = PrepareReport(flight, nextLeg);
            try
            {
                 _webSockets.SendAllAsync(report);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private WSClientReport PrepareReport(IFlight flight, Leg leg)
        {
            /* Input: changed flight and approved next leg or null(e.g flight registered notification).
             * Report object which is the data sent to all active websocket connections.*/
            FlightReport report = new FlightReport();
            if (leg == null)
            {
                report.Airplane = flight.GetAirplane().Name;
                report.CurrentLegId = flight.GetProcess().CurrentLeg.Id;
                report.CurrentLegType = flight.GetProcess().CurrentLeg.Type.ToString();
                report.Process = flight.GetProcess().ToString();
            }
            else
            {
                report.Airplane = flight.GetAirplane().Name;
                report.CurrentLegId = leg.Id;
                report.CurrentLegType = leg.Type.ToString();
                report.Process = flight.GetProcess().ToString();
            }
            return report;
        }
        public Collection<Leg> GetLayOut()
        {
            //Get airport layout.
            return _airportLayout;
        }

        public async Task RequestAsync(IFlight flight)
        {
            /* Receives a request from an active flight and adds it to the queue based on IsCritical status. */
            _logger.LogAction($"Flight {flight.GetAirplane().Name} is asking permission to move to next leg.");
            Register(flight);
            if (!_requestQueue.Contains(flight))
            {
                lock (_requestQueue)
                {
                    if (flight.GetAirplane().IsCritical && _requestQueue.Count > 0)
                        AddFirst(flight);
                    else
                        _requestQueue.Enqueue(flight);
                }
                CheckStart();
            }
        }
        private void AddFirst(IFlight flight)
        {
            /* Add request to top of queue. */
            lock (_requestQueue)
            {
                List<IFlight> tempList = new List<IFlight>();
                tempList.Add(flight);
                foreach (IFlight item in _requestQueue.ToList())
                {
                    tempList.Add(item);
                }
                _requestQueue = new Queue<IFlight>(tempList);
            }
        }

        private void CheckStart()
        {
            /* Checks if timer is running. Starts timer if necessary. */
            if (!_started)
                Start();
        }

        private void Start()
        {
            /* Starts timer. */
            _started = true;
            _timer.Start();
        }
        private void TimerInterval(object sender, ElapsedEventArgs e)
        {
            /* Process requestes based on requests queue. 
             * Approves and update all connected ws clients, or,
                returns request to queue. */
            if (_requestQueue.Count > 0)
            {
                IFlight flight = _requestQueue.Dequeue();
                Leg res = ProcessRequest(flight);
                if (res == null)
                    lock (_requestQueue)
                        _requestQueue.Enqueue(flight);
                else
                {
                    ApprovedAsync(flight, res);
                    UpdateClientsFlightAsync(flight, res);
                }
            }
            else
                Stop();
        }
        private Leg ProcessRequest(IFlight flight)
        {
            /* Decides if flight can move to next leg based on its process and its position in specified airport. */
            if (flight == null)
                return null;
            //If flight is departing and finished Strip leg it left the airport.
            if (flight.GetProcess() is DepartureProcess && flight.GetProcess().CurrentLeg?.Type == LegType.Strip)
                return new Leg { Id = 0, MaxCapacity = 1, CurrentCapacity = 0, SpeedLimit = flight.GetAirplane().Speed, LengthKM = 0, Type = LegType.LeftAirport, NextLegs = new Collection<Leg>() };

            //3. Trying to get next availalbe leg based on process expected next leg type.
            LegType nextLegType = flight.GetProcess().GetNextLegType(flight.GetProcess().CurrentLeg.Type);
            Leg nextLeg = flight.GetProcess().CurrentLeg.NextLegs.FirstOrDefault(leg => leg.Type == nextLegType && leg.CurrentCapacity < leg.MaxCapacity);

            //If 3. failed trying to get next available leg based on current leg type.
            if (nextLeg == null)
            {
                nextLegType = flight.GetProcess().CurrentLeg.Type;
                nextLeg = flight.GetProcess().CurrentLeg.NextLegs.FirstOrDefault(leg => leg.Type == nextLegType && leg.CurrentCapacity < leg.MaxCapacity);
            }

            /*If flight is landing and about to enter runway, need to make sure it can clear the runway.
                (not to hold departing fligths when available capacity on runway is 1.)*/
            if (flight.GetProcess() is LandingProcess && nextLeg != null && nextLeg.Type == LegType.Strip)
            {
                LegType nextNextLegType = flight.GetProcess().GetNextLegType(LegType.Strip);
                Leg nextNextLeg = nextLeg.NextLegs.FirstOrDefault(leg => leg.Type == nextNextLegType && leg.CurrentCapacity < leg.MaxCapacity);
                if (nextNextLeg != null)
                    return nextLeg;
                else
                    return null;
            }

            /* In case of available capacity of 1 in hanger, 1 spot is reserved for departing flights. */
            if (flight.GetProcess() is DepartureProcess && nextLeg != null && nextLeg.Type == LegType.Hanger && nextLeg.CurrentCapacity >= nextLeg.MaxCapacity - 1)
                return null;

            return nextLeg;
        }
        private async Task ApprovedAsync(IFlight flight, Leg nextLeg)
        {
            /* Input: flight that was cleared to enter next leg and destination leg.
             * Moves flight from current leg to next leg.
                Notify flight and logs.*/
            await _logger.LogAction($"Flight {flight.GetAirplane().Name} approved to move to leg {nextLeg.Id}.");
            CheckOut(flight.GetProcess().CurrentLeg);
            CheckIn(nextLeg);
            await _logger.LogAction($"Flight {flight.GetAirplane().Name} moved from leg {flight.GetProcess().CurrentLeg.Id} to leg {nextLeg.Id}.");
            flight.Approve(nextLeg);
        }
        private void Stop()
        {
            // Stops timer.
            _started = false;
            _timer.Stop();
        }
        private void CheckOut(Leg leg)
        {
            // Updates current leg current capacity property.
            lock (_airportLayout)
            {
                leg.CurrentCapacity -= 1;
            }
        }
        private void CheckIn(Leg leg)
        {
            // Updates current leg current capacity property.
            lock (_airportLayout)
                leg.CurrentCapacity += 1;
        }

        public async Task DoneAsync(IFlight flight)
        {
            /* Receives a notification from active flight that it is done with its process.
             * Updates all active ws clients.
                Update current leg.
                 Remove flights. */
            await _logger.LogAction($"Control tower received that flight {flight.GetAirplane().Name} is done.");
            await UpdateClientsFlightAsync(flight, null);
            CheckOut(flight.GetProcess().CurrentLeg);
            UnRegister(flight);
        }
    }
}
