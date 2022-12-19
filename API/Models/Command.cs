using static API.Compumat;

namespace API.Models {
    public class Command {
        public string CommandName { get; set; }
        public string CommandString { get; set; }
        public CompumatType[] ValidCompumats { get; set; }
        public CommandArgument[] Arguments { get; set; }

    }

    public class CommandArgument {
        public string ArgName { get; set; }
        public string ArgType { get; set; }
        public string? ArgPlaceholder { get; set; }
        public dynamic[]? ValidArgs { get; set; }
        public string? MinArg { get; set; }
        public string? MaxArg { get; set; }
        public bool Required { get; set; }
    }
}
