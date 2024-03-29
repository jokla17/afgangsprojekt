﻿using System;
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
        public LogMessageType MessageType { get; set; }

        public static LogEntry ParseLogEntry(string rawLogEntry) {
            string[] logSplit = rawLogEntry.Split(';');

            CultureInfo provider = CultureInfo.InvariantCulture;
            string format = "dd.MM.yy;HH:mm:ss";
            string dateString = $"{logSplit[0]};{logSplit[1]}";

            LogMessageType logMessageType = LogMessageTypes.EvaluateMessageType(logSplit[4]);

            return new LogEntry {
                Timestamp = DateTime.ParseExact(dateString, format, provider),
                CompumatId = logSplit[2],
                Message = logSplit[4],
                MessageType = logMessageType
            };
        }
    }
}
