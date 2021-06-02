using AirportSimulator2.LogicalEntities;
using AirportModels;
using AirportSimulator2.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AirportSimulator2.BL
{
    public class AirportLogic : IAirportLogic
    {
        private IControlTower _controlTower;
        private IAirportLog _logger;
        //private static Collection<Task> _activeTasks;
        public AirportLogic(IControlTower controlTower, IAirportLog logger)
        {
            //_activeTasks = new Collection<Task>();
            _controlTower = controlTower;
            _logger = logger;
        }
        public async Task ArrivalAsync(Airplane airplane, int eta)
        {
            //Receives a landing airplane and eta to ariport, turns it into a flight and starts the landing process.
            _logger.LogAction($"Flight {airplane.Name} arrived to airport for landing.");
            LandingProcess landingProcess = new LandingProcess
            {
                CurrentLeg = new Leg
                {
                    Id = 0,
                    CurrentCapacity = 1,
                    MaxCapacity = 1,
                    SpeedLimit = airplane.Speed,
                    Type = LegType.WaitingForEntrance,
                    LengthKM = eta * airplane.Speed,
                    NextLegs = new Collection<Leg>(_controlTower?.GetLayOut().Where(leg => leg.Type == LegType.EntryPoint).ToList())
                },
                FlightPlan = new List<LegType> { LegType.WaitingForEntrance, LegType.EntryPoint, LegType.ApproachLanding, LegType.Strip, LegType.TransportToLoad, LegType.Load, LegType.Hanger }
            };
            
            IFlight newFlight = new Flight { _airplane = airplane, _process = landingProcess, _controlTower = this._controlTower, _logger = this._logger };
            newFlight.Go();
            /*
            Task t = Task.Run(() => newFlight.Go());
            _activeTasks.Add(t);
            //t.GetAwaiter().OnCompleted(() => _activeTasks.Remove(t));
            t.GetAwaiter().OnCompleted(() => Done(t));*/
        }

        public async Task DepartureAsync(Airplane airplane)
        {
            //Receives a departing airplane, turns it into a departing flight and starts departing process.
            _logger.LogAction($"Flight {airplane.Name} arrived to airport for departing.");
            DepartureProcess departureProcess = new DepartureProcess
            {
                CurrentLeg = new Leg
                {
                    Id = 0,
                    CurrentCapacity = 1,
                    MaxCapacity = 1,
                    SpeedLimit = airplane.Speed,
                    LengthKM = 0,
                    Type = LegType.WaitingForEntrance,
                    NextLegs = new Collection<Leg>(_controlTower.GetLayOut().Where(leg => leg.Type == LegType.Hanger).ToList())
                },
                FlightPlan = new List<LegType> { LegType.WaitingForEntrance, LegType.Hanger, LegType.Load, LegType.TransportToStrip, LegType.Strip, LegType.LeftAirport }
            };
            IFlight newFlight = new Flight { _airplane = airplane, _controlTower = _controlTower, _process = departureProcess, _logger = _logger };
            newFlight.Go();
            /*
            //Task.Run(() => newFlight.Go());
            Task t = Task.Run(() => newFlight.Go());
            _activeTasks.Add(t);
            //t.GetAwaiter().OnCompleted(() => _activeTasks.Remove(t));
            t.GetAwaiter().OnCompleted(() => Done(t));*/
        }

        /*
        private void Done(Task t)
        {
            int i = 5;
            _activeTasks.Remove(t);
            i = 3;
        }
        */
    }
}
