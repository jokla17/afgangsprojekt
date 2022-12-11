using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompumatServer {
    public class LogMessageTypes {
        public static readonly Regex GATE_BoomOutgoing = new Regex("Bil med nummerplade [A-Za-z0-9]+ kører ud");
        public static readonly Regex GATE_BoomIncoming = new Regex("Bil med nummerplade [A-Za-z0-9]+ kører ind");
        public static readonly Regex GATE_UnknownNumberplate = new Regex("Ukendt nummerplade [A-Za-z0-9]+");
        public static readonly Regex GATEWAY_Booted = new Regex("Booted: [0-9]+");
        public static readonly Regex GATEWAY_Alive = new Regex("[0-9]+-Gateway Alive-([0-9]+(\\.[0-9]+)+) - ([0-9]+(:[0-9]+)+)");
        public static readonly Regex GATEWAY_UnitLogin = new Regex("[0-9]+-[a-zA-Z]+-\\|[0-9]+\\| [a-zA-Z]+ logged in");
        public static readonly Regex GATEWAY_UnitDisconnected = new Regex("[0-9]+-[a-zA-Z]+-\\|[0-9]+\\| [a-zA-Z]+ disconnected");
        public static readonly Regex UNIT_Status = new Regex("Lun:[0-9]+ - output no\\.:[A-Za-z0-9]+ - Status:[A-Za-z0-9]+");
        public static readonly Regex Network_NewConnection = new Regex("[0-9]+-Login accepted from [0-9]+\\.[0-9]+\\.[0-9]+\\.[0-9]+ - Gateway Serial number: [0-9]+ - MatNo: [0-9]+");
        public static readonly Regex Network_Diagnostics = new Regex("Diagnostic: [0-9]+\\.[0-9]+\\.[0-9]+.*-:.*");
        public static readonly Regex Network_ServerStopped = new Regex("Server stopped! - Station:[A-Za-z0-9]+");
        public static readonly Regex Network_ServerStarted = new Regex("Server started: [A-Za-z0-9]+.*");
        public static readonly Regex Network_DeviceList = new Regex(".*:DEVICELIST:DEVICES=(([+-]?(?=\\.\\d|\\d)(?:\\d+)?(?:\\.?\\d*))(?:[eE]([+-]?\\d+))?((,([+-]?(?=\\.\\d|\\d)(?:\\d+)?(?:\\.?\\d*))(?:[eE]([+-]?\\d+))?)+)\\/){0,};");

        private static readonly Dictionary<Regex, LogMessageType> messageTypes = new Dictionary<Regex, LogMessageType>() {
            { GATE_BoomOutgoing, LogMessageType.GATE_BoomOutgoing },
            { GATE_BoomIncoming, LogMessageType.GATE_BoomIncoming },
            { GATE_UnknownNumberplate, LogMessageType.GATE_UnknownNumberplate },
            { GATEWAY_Booted, LogMessageType.GATEWAY_Booted },
            { GATEWAY_Alive, LogMessageType.GATEWAY_Alive },
            { GATEWAY_UnitLogin, LogMessageType.GATEWAY_UnitLogin },
            { GATEWAY_UnitDisconnected, LogMessageType.GATEWAY_UnitDisconnected },
            { UNIT_Status, LogMessageType.UNIT_Status },
            { Network_NewConnection, LogMessageType.Network_NewConnection },
            { Network_Diagnostics, LogMessageType.Network_Diagnostics },
            { Network_ServerStopped, LogMessageType.Network_ServerStopped },
            { Network_ServerStarted, LogMessageType.Network_ServerStarted },
            { Network_DeviceList, LogMessageType.Network_DeviceList },

        };

        // Parses the message (body of the log-entry) against a dictionary of Regex-pattern-keys with corresponding MessageType-values, to figure out what type of log-entry this is.
        public static LogMessageType EvaluateMessageType(string message) {
            LogMessageType type = LogMessageType.UNKNOWN;
            foreach (KeyValuePair<Regex, LogMessageType> entry in messageTypes) {
                var matches = entry.Key.Matches(message);
                if(matches.Count() > 0) type = entry.Value;
            }
            return type;
        }
    }

    public enum LogMessageType {
        UNKNOWN = -1,
        GATE_BoomOutgoing = 0,
        GATE_BoomIncoming = 1,
        GATE_UnknownNumberplate = 2,
        GATEWAY_Booted = 3,
        GATEWAY_Alive = 4,
        GATEWAY_UnitLogin = 5,
        GATEWAY_UnitDisconnected = 6,
        UNIT_Status = 7,
        Network_NewConnection = 8,
        Network_Diagnostics = 9,
        Network_ServerStopped = 10,
        Network_ServerStarted = 11,
        Network_DeviceList = 12,
    }
}
