using System.Collections.ObjectModel;

namespace AirportModels
{
    public class Leg
    {
        public int Id { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentCapacity { get; set; }
        public int SpeedLimit { get; set; }
        public int LengthKM { get; set; }
        public LegType Type { get; set; }
        public Collection<Leg> NextLegs { get; set; }
    }
}
