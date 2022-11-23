using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using API.Services;
using API.RabbitMQ;

namespace API.Controllers
{
    [EnableCors("myPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class CompumatController: ControllerBase
    {
        private readonly ILogger<MapController> _logger;
        private readonly CompumatService _compumatService;
        private readonly CommunicationService _communicationService;
        private readonly RabbitService _rabbitService;

        public CompumatController(ILogger<MapController> logger, CompumatService compumatService, CommunicationService communicationService, RabbitService rabbitService)
        {
            _logger = logger;
            _compumatService = compumatService;
            _communicationService = communicationService;
            _rabbitService = rabbitService;
        }

        [HttpGet("ReadOne")]
        public async Task<IActionResult> ReadOne(int id)
        {
            Compumat result = await _compumatService.GetCompumat(id);
            if (result == null) return NotFound(id);
            return Ok(result);
        }
        
        [HttpGet("ReadAll")]
        public async Task<IActionResult> ReadAll()
        {
            List<Compumat> result = await _compumatService.GetAllCompumats();
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(Compumat compumat)
        {
            Compumat result = await _compumatService.CreateCompumat(compumat);
            if (result == null) return NotFound();
            return Created("Create", result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody]Compumat compumat)
        {
            Compumat result = await _compumatService.UpdateCompumat(compumat);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            string result = await _compumatService.DeleteCompumat(id);
            if (result == null) return NotFound(id);
            return Ok(result);
        }

        [HttpPost("TestCompumatServer")]
        public async Task<IActionResult> TestCPS([FromBody] Dictionary<string,string> message) {
            string resp = this._communicationService.SendObject(message);
            return Ok(resp);
        }

        [HttpPost("TestCompumatServerPoll")]
        public async Task<IActionResult> TestCPSPoll() {
            this._communicationService.PollServer();
            return Ok();
        }

        [HttpGet("StopPolling")]
        public async Task<IActionResult> StopPolling() {
            this._communicationService.StopPolling();
            return Ok();
        }

        [HttpPost("TestRabbit")]
        public async Task<IActionResult> TestRabbit(string[] tasks) {
            _rabbitService.SendTask(tasks);
            return Ok();
        }

    }
}
