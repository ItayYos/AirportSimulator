using AirportModels;

namespace AirportSimulator2.LogicalEntities
{
    public interface IFlight
    {
        void Go();
        void Approve(Leg nextLeg);
        AirportProcess GetProcess();
        Airplane GetAirplane();
    }
}
