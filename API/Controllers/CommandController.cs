using API.Models;
using API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [EnableCors("myPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class CommandController : ControllerBase {
        private readonly ILogger<CommandController> _logger;
        private readonly CommandService _commandService;
        public CommandController(ILogger<CommandController> logger, CommandService commandService) {
            _logger = logger;
            _commandService = commandService;
        }

        [HttpGet("GetCommands/{user}")]
        public async Task<IActionResult> GetCommands([FromRoute] string user) {
            List<Command> result = await _commandService.GetCommands(user);
            return Ok(result);
        }

        [HttpPost("ExecuteCommand/{user}")]
        public async Task<IActionResult> ExecuteCommand([FromBody] Command command, [FromRoute] string user) {
            bool result = await _commandService.ExecuteCommand(command, user);
            if (result == true) {
                return Ok(result);
            } else {
                return BadRequest(result);
            }
        }
    }
}
