using AirportSimulator2.BL;
using AirportModels;
using Microsoft.AspNetCore.Mvc;

namespace AirportSimulator2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AirplaneController : ControllerBase
    {
        private IAirportLogic _airportLogic;
        public AirplaneController(IAirportLogic airportLogic)
        {
            _airportLogic = airportLogic;
        }
        [HttpPost]
        [Route("/Airplane/Arrival")]
        public void Arrival(Airplane airplane, int eta)
        {
            _airportLogic.ArrivalAsync(airplane, eta);
        }
        [HttpPost]
        [Route("/Airplane/Departure")]
        public void Departure(Airplane airplane)
        {
            _airportLogic.DepartureAsync(airplane);
        }
    }
}
