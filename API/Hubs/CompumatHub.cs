using Microsoft.AspNetCore.SignalR;

namespace API.Hubs {
    public class CompumatHub : Hub {
        public async Task SendMessage(string message) {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendCompumat(Compumat compumat) {
            await Clients.All.SendAsync("ReceiveCompumat", compumat);
        }

        public async Task SendCompumats(List<Compumat> compumats) {
            await Clients.All.SendAsync("ReceiveCompumats", compumats);
        }
    }
}
