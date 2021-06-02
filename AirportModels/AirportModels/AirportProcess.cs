using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportModels
{
    public abstract class AirportProcess
    {
        public List<LegType> FlightPlan;
        public Leg CurrentLeg;
        public abstract Action GetWork(int numOfPassangers, int speed);
        public abstract LegType GetNextLegType(LegType legType);
        public bool IsDone()
        {
            if (CurrentLeg?.Type == FlightPlan?.Last())
                return true;
            else
                return false;
        }
    }
}
