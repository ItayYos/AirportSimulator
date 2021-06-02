using AirportModels;
using System.Collections.ObjectModel;

namespace AirportSimulator2.AirportLayoutBuilder
{
    public class AirportBuilder : IAirportBuilder
    {
        public Collection<Leg> Build(int hangerMaxCapacity)
        {
            if (hangerMaxCapacity < 2)
                hangerMaxCapacity = 2;
            Leg l1 = new Leg { Id = 1, MaxCapacity = 1, CurrentCapacity = 0, SpeedLimit = 300, LengthKM = 2, Type = LegType.EntryPoint };
            Leg l2 = new Leg { Id = 2, MaxCapacity = 1, CurrentCapacity = 0, SpeedLimit = 200, LengthKM = 2, Type = LegType.ApproachLanding };
            Leg l3 = new Leg { Id = 3, MaxCapacity = 1, CurrentCapacity = 0, SpeedLimit = 150, LengthKM = 2, Type = LegType.ApproachLanding };
            Leg l4 = new Leg { Id = 4, MaxCapacity = 1, CurrentCapacity = 0, SpeedLimit = 100, LengthKM = 3, Type = LegType.Strip };
            Leg l5 = new Leg { Id = 5, MaxCapacity = 1, CurrentCapacity = 0, SpeedLimit = 80, LengthKM = 1, Type = LegType.TransportToLoad };
            Leg l6 = new Leg { Id = 6, MaxCapacity = 1, CurrentCapacity = 0, SpeedLimit = 0, LengthKM = 0, Type = LegType.Load };
            Leg l7 = new Leg { Id = 7, MaxCapacity = 1, CurrentCapacity = 0, SpeedLimit = 0, LengthKM = 0, Type = LegType.Load };
            Leg l8 = new Leg { Id = 8, MaxCapacity = 1, CurrentCapacity = 0, SpeedLimit = 90, LengthKM = 1, Type = LegType.TransportToStrip };
            Leg l9 = new Leg { Id = 9, MaxCapacity = hangerMaxCapacity, CurrentCapacity = 0, SpeedLimit = 0, LengthKM = 0, Type = LegType.Hanger };

            l1.NextLegs = new Collection<Leg> { l2 };
            l2.NextLegs = new Collection<Leg> { l3 };
            l3.NextLegs = new Collection<Leg> { l4 };
            l4.NextLegs = new Collection<Leg> { l5 };
            l5.NextLegs = new Collection<Leg> { l6, l7 };
            l6.NextLegs = new Collection<Leg> { l8, l9 };
            l7.NextLegs = new Collection<Leg> { l8, l9 };
            l8.NextLegs = new Collection<Leg> { l4 };
            l9.NextLegs = new Collection<Leg> { l6, l7 };

            Collection<Leg> ans = new Collection<Leg> { l1, l2, l3, l4, l5, l6, l7, l8, l9 };
            return ans;
        }
    }
}
