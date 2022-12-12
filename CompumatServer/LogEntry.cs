using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompumatServer {
    public class LogEntry {
        public DateTime Timestamp { get; set; }
        public string CompumatId { get; set; }
        public string Message { get; set; }

        public LogEntry ParseLogEntry(string rawLogEntry) {
            string[] logSplit = rawLogEntry.Split(';');
            CultureInfo provider = CultureInfo.InvariantCulture;
            string format = "dd.MM.yyyy HH:mm:ss"
            return new LogEntry {
                Timestamp = DateTime.ParseExact(logSplit[0])
        }
    }
}
