using AirportModels;
using System.Threading.Tasks;

namespace AirportSimulator2.BL
{
    public interface IAirportLogic
    {
        Task ArrivalAsync(Airplane airplane, int eta);
        Task DepartureAsync(Airplane airplane);
    }
}
