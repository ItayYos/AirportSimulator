using AirportModels;
using System;
using System.Threading;

namespace AirportSimulator2.LogicalEntities
{
    public class LandingProcess : AirportProcess
    {
        public override LegType GetNextLegType(LegType legType)
        {
            /* Receives current leg type.
             * Returns next leg type based on current leg type and flight plan.
             * Returns LegType.WaitingForEntrance if index out of bounds. */
            int index = FlightPlan.IndexOf(legType);
            if (index >= 0 && index < FlightPlan.Count - 1)
            {
                LegType nextLegType = FlightPlan[index + 1];
                return nextLegType;
            }
            if (index == FlightPlan.Count - 1)
                return FlightPlan[index];
            return LegType.WaitingForEntrance;
        }

        public override Action GetWork(int numOfPassangers, int speed)
        {
            /* Receives number of passangers and speed.
             * Returns an action that simulates current sub process. */
            int time = 0;
            if (CurrentLeg?.Type == LegType.Load)
            {
                time = numOfPassangers * 1; //assuming it takes a passnager 1 minute to get on or get off the plane.
            }
            else if (speed > 0)
            {
                if (CurrentLeg.SpeedLimit < speed && CurrentLeg.SpeedLimit > 0)
                    time = CurrentLeg.LengthKM / CurrentLeg.SpeedLimit % 1;
                else
                    time = CurrentLeg.LengthKM / speed % 1;
            }
            time *= 60000;
            time = Validate(time);
            return () => Thread.Sleep(time);
        }

        private int Validate(int time)
        {
            /* Make sure time is in the following range [2,5].
             * For simulation purposes. */
            if (time > 5000)
                return 5000;
            if (time < 2000)
                return 2000;
            return time;
        }
    }
}
