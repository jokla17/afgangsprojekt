using API.Models;
using Newtonsoft.Json;

namespace API.Repositories {
    public class CommandRepository {

        public async Task<List<Command>> GetCommands() {
            string jsonCmds = this.GetJsonCommands();
            Console.WriteLine(jsonCmds);
            List<Command>? commands = JsonConvert.DeserializeObject<List<Command>>(jsonCmds);
            return commands;
        }

        private string GetJsonCommands() {
            using (StreamReader r = new StreamReader("./cmds.json")) {
                return r.ReadToEnd();
            }
        }

    }
}
