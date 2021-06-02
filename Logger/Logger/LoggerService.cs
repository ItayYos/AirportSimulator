using System;
using System.IO;
using System.Threading.Tasks;

namespace Logger
{
    public class LoggerService : ILoggerService
    {
        public string Path { get; set; }

        public LoggerService()
        {
            Path = @"C:\Programs\Final Project\Log Files\";
            Path += $"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Year}.txt";
        }

        public LoggerService(string path)
        {
            Path = path;
        }
        public async Task Log(string message)
        {
            using (FileStream fileStream = new FileStream(Path, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fileStream))
                {
                    string timeStamp = GetTimeStamp();
                    await sw.WriteLineAsync(timeStamp + ": " + message);
                }
            }
        }
        public async Task Log(string message, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fileStream))
                {
                    string timeStamp = GetTimeStamp();
                    await sw.WriteLineAsync(timeStamp + ": " +message);
                }
            }
        }
        private string GetTimeStamp()
        {
            return DateTime.Now.ToString();
        }
    }

}
