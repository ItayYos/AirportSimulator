
namespace AirportSimulator2.WS
{
    public class FlightReport : WSClientReport
    {
        public string Airplane { get; set; }
        public int CurrentLegId { get; set; }
        public string CurrentLegType { get; set; }
        public string Process { get; set; }
    }
}
