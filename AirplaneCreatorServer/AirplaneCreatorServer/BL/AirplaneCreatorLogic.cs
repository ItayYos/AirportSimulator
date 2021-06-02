using AirportModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Timers;

namespace AirplaneCreatorServer.BL
{
    public class AirplaneCreatorLogic : IAirplaneCreatorLogic
    {
        private Timer _timer;
        private string[] _airLineNames;
        private HttpClient _httpClient;
        private string _uri;

        public AirplaneCreatorLogic()
        {
            _timer = new Timer();
            _airLineNames = new string[] { "LY", "AF", "IZ", "6H", "A3", "TK", "LH" };
            _httpClient = new HttpClient();
            _uri = "http://localhost:50677/Airplane";
        }
        public void Start()
        {
            // Instantiate timer, sets interval, elapesed callback and starts the timer.
            _timer = new Timer(3000);
            _timer.Elapsed += TimerElapesed;
            _timer.Start();
        }
        public void Stop()
        {
            //Stops timer.
            _timer.Dispose();
        }
        private void TimerElapesed(object sender, EventArgs e)
        {
            /* Generates an airplane, assign departing or landing orientation and sends an http request accordingly. */
            _timer.Stop();
            Airplane newAirplane = GenerateAirplane();
            bool departure = DeciceArrivalOrDeparture();
            if (departure)
                Departure(newAirplane);
            else
            {
                int eta = new Random().Next(1,4);
                Arrival(newAirplane, eta);
            }
            _timer.Start();
        }
        private void Arrival(Airplane airplane, int eta)
        {
            // Sends an http request of a landing flight.
            string json = JsonConvert.SerializeObject(airplane);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            string uri = _uri + "/Arrival" + $"?eta={eta}";
            _httpClient.PostAsync(uri, content);
        }
        private void Departure(Airplane airplane)
        {
            // Sends an http request of a departing flight.
            string json = JsonConvert.SerializeObject(airplane);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            string uri = _uri + "/Departure";
            _httpClient.PostAsync(uri, content);
        }
        private bool DeciceArrivalOrDeparture()
        {
            // 1/3 chance returns true, otherwise false.
            return new Random().Next(1, 3) == 1;
        }
        private Airplane GenerateAirplane()
        {
            /* Generates an airplane of random properties and returns it. */
            int nameIndex = new Random().Next(0, _airLineNames.Length);
            string name = _airLineNames[nameIndex];
            name += new Random().Next(100, 1000).ToString();
            int maxSpeed = new Random().Next(300, 900);
            int passangersNum = new Random().Next(1, 800);
            bool critical = new Random().Next(0, 5) == 4;   // 20% for flight to be critical.
            Airplane airplane = new Airplane { Name = name, Speed = maxSpeed, NumOfPassangers = passangersNum, IsCritical = critical };

            return airplane;
        }
    }
}
