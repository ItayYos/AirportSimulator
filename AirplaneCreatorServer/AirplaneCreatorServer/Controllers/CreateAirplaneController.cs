using AirplaneCreatorServer.BL;
using Microsoft.AspNetCore.Mvc;

namespace AirplaneCreatorServer.Controllers
{
    [ApiController]
    public class CreateAirplaneController : ControllerBase
    {
        private IAirplaneCreatorLogic _airportCreatorLogic;

        public CreateAirplaneController(IAirplaneCreatorLogic airplaneCreatorLogic)
        {
            _airportCreatorLogic = airplaneCreatorLogic;
        }

        [Route("/Start")]
        public void Start()
        {
            _airportCreatorLogic.Start();
        }

        [Route("/Stop")]
        public void Stop()
        {
            _airportCreatorLogic.Stop();
        }
    }
}
