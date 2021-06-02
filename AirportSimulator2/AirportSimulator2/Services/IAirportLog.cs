using System.Threading.Tasks;

namespace AirportSimulator2.Services
{
    public interface IAirportLog
    {
        Task LogError(string msg);
        Task LogAction(string msg);
    }
}
