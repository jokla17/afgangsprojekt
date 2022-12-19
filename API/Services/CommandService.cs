using API.Models;
using API.Repositories;

namespace API.Services {
    public class CommandService {
        private readonly CommunicationService _communicationService;
        private readonly CommandRepository _commandRepository;
        public CommandService(CommunicationService communicationService, CommandRepository commandRepository) {
            _communicationService = communicationService;
            _commandRepository = commandRepository;
        }

        public async Task<List<Command>> GetCommands(string user) {
            if (this.IsAdmin(user)) {
                return await this._commandRepository.GetCommands();
            } else {
                // Ask repository for some commands
                throw new NotImplementedException();
            }

            //return result
            throw new NotImplementedException();
        }

        public async Task<bool> ExecuteCommand(Command command, string user) {
            if (this.IsAdmin(user)) {
                // Execute command --> communicationController
                throw new NotImplementedException();
            } else {
                return false;
            }
        }
        private bool IsAdmin(string user) {
            if (string.Compare(user, "admin_22", StringComparison.OrdinalIgnoreCase) == 0) {
                return true;
            } else {
                return false;
            }
        }
    }
}
