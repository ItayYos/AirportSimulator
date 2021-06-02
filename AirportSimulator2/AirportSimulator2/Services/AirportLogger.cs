using Logger;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AirportSimulator2.Services
{
    public class AirportLogger : IAirportLog
    {
        private ILoggerService _logger;
        public AirportLogger(ILoggerService logger)
        {
            _logger = logger;
        }
        public async Task LogError(string msg)
        {
            /* Logs an error to Errors.txt file, created in current project's folder. */
            string fileName = "Errors.txt";
            string path = Path.Combine(Environment.CurrentDirectory, fileName);
            await _logger.Log(msg, path);
        }

        public async Task LogAction(string msg)
        {
            // Logs an action to Actions.txt file, created in current project's folder.
            string fileName = "Actions.txt";
            string path = Path.Combine(Environment.CurrentDirectory, fileName);
            await _logger.Log(msg, path);
        }
    }
}
