using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using API.Services;

namespace API.Controllers
{
    [EnableCors("myPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class MapController: ControllerBase
    {

        private readonly ILogger<MapController> _logger;
        private readonly MapService _mapService;

        public MapController(ILogger<MapController> logger, MapService mapService)
        {
            _logger = logger;
            _mapService = mapService;
        }

        [HttpGet("ReadOne")]
        public async Task<IActionResult> ReadOne(int id)
        {
            Map result = await _mapService.GetMap(id);
            return Ok(result);
        }
        
        [HttpGet("ReadAll")]
        public async Task<List<Map>> ReadAll()
        {
            List<Map> result = await _mapService.GetAllMaps();
            return result;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Map map)
        {
            Map result = await _mapService.CreateMap(map);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Map map)
        {
            Map result = await _mapService.UpdateMap(map);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            string result = await _mapService.DeleteMap(id);
            return Ok(result);
        }


    }
}
