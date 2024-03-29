﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using API.Services;
using API.RabbitMQ;
using Microsoft.AspNetCore.SignalR;
using API.Hubs;

namespace API.Controllers
{
    [EnableCors("myPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class CompumatController: ControllerBase
    {
        private readonly ILogger<MapController> _logger;
        private readonly CompumatHub _hub;
        private readonly CompumatService _compumatService;
        private readonly CommunicationService _communicationService;
        private readonly RabbitService _rabbitService;

        public CompumatController(ILogger<MapController> logger, CompumatService compumatService, CommunicationService communicationService, RabbitService rabbitService, CompumatHub hub)
        {
            _logger = logger;
            _compumatService = compumatService;
            _communicationService = communicationService;
            _rabbitService = rabbitService;
            _hub = hub;
        }

        [HttpPost("TestRabbit")]
        public async Task<IActionResult> TestSignalR([FromBody]string[] bindingKeys) 
        {
            _rabbitService.ReceiveTopic(bindingKeys);
            return Ok();
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
            await _hub.AddCompumat(result);
            return Created("Create", result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody]Compumat compumat)
        {
            Compumat result = await _compumatService.UpdateCompumat(compumat);
            if (result == null) return NotFound();
            await _hub.ChangeCompumat(result);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            string result = await _compumatService.DeleteCompumat(id);
            if (result == null) return NotFound(id);
            await _hub.RemoveCompumat(result);
            return Ok(result);
        }

        [HttpPost("TestCompumatServer")]
        public async Task<IActionResult> TestCPS([FromBody] Dictionary<string,string> message) {
            string resp = this._communicationService.SendObjectToCompumatServer(message);
            return Ok(resp);
        }

        [HttpPost("TestCompumatServerPoll")]
        public async Task<IActionResult> TestCPSPoll() {
            this._communicationService.PollCompumatServer();
            return Ok();
        }

        [HttpGet("StopPolling")]
        public async Task<IActionResult> StopPolling() {
            this._communicationService.StopPollingCompumatServer();
            return Ok();
        }

        [HttpGet("GetLog")]
        public async Task<IActionResult> GetLog() {
            List<LogEntry> log = await this._communicationService.GetLog();
            return Ok(log);
        }

        [HttpGet("GetCompumatLog/{compumatId}")]
        public async Task<IActionResult> GetCompumatLog([FromRoute] string compumatId) {
            List<LogEntry> log = await this._communicationService.GetCompumatLog(compumatId);
            return Ok(log);
        }

        [HttpPost("TestRabbit2")]
        public async Task<IActionResult> TestRabbit(string[] tasks) {
            _rabbitService.SendTask(tasks);
            return Ok();
        }

    }
}
