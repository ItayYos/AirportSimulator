using AirportSimulator2.BL;
using AirportModels;
using AirportSimulator2.Services;
using System;

namespace AirportSimulator2.LogicalEntities
{
    public class Flight : IFlight
    {
        /* Simulates an active flight landing or departing from airport. */
        public Airplane _airplane;
        public AirportProcess _process;
        public IControlTower _controlTower;
        public IAirportLog _logger;
        
        public Airplane GetAirplane() { return _airplane; }
        public AirportProcess GetProcess() { return _process; }
        public void Go()
        {
            /* Getting ready based on current leg sub process.
             * Ask permission to gain access to next leg from control tower, or, notifying control tower that process is done.*/           
            GetReady();
            if (!_process.IsDone())
                AskForNextLeg();
            else
                Done(); 
        }
        public void Approve(Leg nextLeg)
        {
            /* Receives response from control tower with our next leg.
                Sets it as our current leg and starts the process of Go function. */
            _logger.LogAction($"Flight {_airplane.Name} recived permission to enter leg {nextLeg.Id}.");
            _process.CurrentLeg = nextLeg;
            _logger.LogAction($"Flight {_airplane.Name} moved to leg {nextLeg.Id}.");
            Go();
        }
        private void GetReady()
        {
            /* Gets a function from process that simulates current leg sub process and execute it. */
            _logger.LogAction($"Flight {_airplane.Name} is getting ready.");
            Action action = _process.GetWork(_airplane.NumOfPassangers, _airplane.Speed);
            action.Invoke();
            _logger.LogAction($"Flight {_airplane.Name} is ready.");
        }
        private void AskForNextLeg()
        {
            // Contact control tower to indicate this flight is ready to move to next leg.
            _logger.LogAction($"Flight {_airplane.Name} asking for next leg.");
            _controlTower.RequestAsync(this);
        }
        private void Done()
        {
            // Notify control tower that this flight process is done.
            _logger.LogAction($"Flight {_airplane.Name} is done.");
            _controlTower.DoneAsync(this);
        }
    }
}
