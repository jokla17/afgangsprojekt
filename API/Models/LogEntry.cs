using System.Globalization;
using System.Xml.Serialization;

namespace API {
    public class LogEntry {
        [XmlElement(ElementName="Timestamp")]
        public DateTime Timestamp { get; set; }

        [XmlElement(ElementName = "CompumatId")]
        public string CompumatId { get; set; }

        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }

        public static LogEntry ParseLogEntry(string rawLogEntry) {
            string[] logSplit = rawLogEntry.Split(';');

            CultureInfo provider = CultureInfo.InvariantCulture;
            string format = "dd.MM.yy;HH:mm:ss";
            string dateString = $"{logSplit[0]};{logSplit[1]}";
            Console.WriteLine(dateString);

            return new LogEntry {
                Timestamp = DateTime.ParseExact(dateString, format, provider),
                CompumatId = logSplit[2],
                Message = logSplit[4]
            };
        }
    }

    [XmlRoot("Log")]
    public class Log {
        public Log() {
            log = new List<LogEntry>();
        }

        [XmlElement("LogEntry")]
        public List<LogEntry>? log { get; set; }

    }
}
