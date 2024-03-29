﻿using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace API.Hubs {
    public class CompumatHub : Hub {

        private readonly IHubContext<CompumatHub> _hub;
        public CompumatHub(IHubContext<CompumatHub> hub) {
            _hub = hub;
        }
        public async Task SendMessage(string message) {
            await _hub.Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task AddCompumat(Compumat compumat) {
            await _hub.Clients.All.SendAsync("AddCompumat", compumat);
        }

        public async Task RemoveCompumat(string compumatId) {
            await _hub.Clients.All.SendAsync("RemoveCompumat", compumatId);
        }

        public async Task ChangeCompumat(Compumat compumat) {
            await _hub.Clients.All.SendAsync("ChangeCompumat", compumat);
        }

        public async Task<bool> TestSignalR(string message) {
            Task t = _hub.Clients.All.SendAsync("TestSignalR", message);
            return t.IsCompleted;
        }

        public async Task CompumatEvent(string routingKey, string message) {
            TimeSpan timestamp = DateTime.UtcNow.Subtract(Process.GetCurrentProcess().StartTime.ToUniversalTime());
            File.AppendAllText("./CompatibilityTestLog.txt", $" {timestamp} > event-message: [ {message} ] \n");
            await _hub.Clients.All.SendAsync(routingKey, message);
        }
    }
}
