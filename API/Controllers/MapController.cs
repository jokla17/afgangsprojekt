using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [EnableCors("myPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class MapController: ControllerBase
    {

        private readonly ILogger<MapController> _logger;

        public MapController(ILogger<MapController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetLongLat")]
        public Map Get()
        {
            return new Map { 
                Id = "01",
                CampSiteName = "NyborgSlot",
                Latitude = 55.31404492554651,
                Longitude = 10.787099052273176
            };
        }
    }
}
