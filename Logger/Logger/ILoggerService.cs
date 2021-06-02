using System.Threading.Tasks;

namespace Logger
{
    public interface ILoggerService
    {
        Task Log(string message);
        Task Log(string message, string path);
    }
}
