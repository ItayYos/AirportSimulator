using AirportSimulator2.LogicalEntities;
using AirportModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AirportSimulator2.BL
{
    public interface IControlTower
    {
        Task RequestAsync(IFlight flight);
        Task DoneAsync(IFlight flight);
        Collection<Leg> GetLayOut();
    }
}
