using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using API.Services;

namespace API.Controllers
{
    [EnableCors("myPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class CompumatController: ControllerBase
    {
        private readonly ILogger<MapController> _logger;
        private readonly CompumatService _compumatService;

        public CompumatController(ILogger<MapController> logger, CompumatService compumatService)
        {
            _logger = logger;
            _compumatService = compumatService;
        }

        [HttpGet("ReadOne")]
        public async Task<IActionResult> ReadOne(int id)
        {
            Compumat result = await _compumatService.GetCompumat(id);
            return Ok(result);
        }
        
        [HttpGet("ReadAll")]
        public async Task<List<Compumat>> ReadAll()
        {
            List<Compumat> result = await _compumatService.GetAllCompumats();
            return result;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(Compumat compumat)
        {
           Compumat result = await _compumatService.CreateCompumat(compumat);
            return Created("Create", result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody]Compumat compumat)
        {
            Compumat result = await _compumatService.UpdateCompumat(compumat);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            string result = await _compumatService.DeleteCompumat(id);
            if (result == null) return NotFound(id);
            return Ok(result);
        }

    }
}
