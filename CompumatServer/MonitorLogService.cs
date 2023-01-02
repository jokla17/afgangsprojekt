using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompumatServer {
    public class MonitorLogService {
        public static readonly string LOG_FILE_PATH = "../../../logs";
        //public static readonly string INPUT_FILE = "/MONITOR-LOG.txt";
        public static readonly string INPUT_FILE = "/CompatibilityTest.txt";
        public static readonly string OUTPUT_FILE = "/LiveLog.txt";

        //private FileStream _logReader = File.OpenRead(LOG_FILE_PATH);
        //private IEnumerable<string> _logReader = File.ReadLines(LOG_FILE_PATH);
        //private FileStream _logWriter = File.OpenWrite(LOG_FILE_PATH);

        public void Start() {
            Thread watcher = new Thread(new ThreadStart(StartFileWatcher));
            Thread simulation = new Thread(new ThreadStart(StartSimulation));

            watcher.Start();
            simulation.Start();
        }

        private void StartLiveMode2() {
            using var watcher = new FileSystemWatcher(LOG_FILE_PATH);
            Console.WriteLine("Started FileWatcher");

            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
            watcher.EnableRaisingEvents = true;
            watcher.Changed += OnChanged;

            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = false;
        }


        private void StartFileWatcher() {
            var wh = new AutoResetEvent(false);
            var fsw = new FileSystemWatcher(LOG_FILE_PATH);
            fsw.Filter = OUTPUT_FILE.Remove(0, 1); // Removes the '/' from the OUTPUT_FILE

            Console.WriteLine($" [FileWatcher] started.. Watching '{fsw.Filter}'\n");
            fsw.EnableRaisingEvents = true;
            fsw.Changed += (s, e) => wh.Set();

            //var fs = new FileStream(LOG_FILE_PATH + OUTPUT_FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var prevLastLine = "";
            while (true) {
                var lastLine = "";

                // If the livelog contains any elements
                //var fileLines = File.ReadLines(LOG_FILE_PATH + OUTPUT_FILE);
                var fileLines = GetFileLines(LOG_FILE_PATH + OUTPUT_FILE);
                if (fileLines.Count() > 0) { 
                    lastLine = fileLines.Last();
                    if (string.Compare(prevLastLine, lastLine) != 0) {
                        prevLastLine = lastLine;
                        Console.WriteLine(" [FileWatcher] New changes: \n" + lastLine + "\n");
                        LogMessageType msgType = LogMessageTypes.EvaluateMessageType(lastLine);
                        var enumMsgType = ((LogMessageType)msgType).ToString();
                        var routingKey = enumMsgType.Replace("_", ".");
                        RabbitService.Emit(routingKey, lastLine);
                    } else {
                        wh.WaitOne(10);
                    }
                } else {
                    wh.WaitOne(1000);
                }
            }

            wh.Close();
        }

        public static IEnumerable<string> GetFileLines(string filepath) {
            var list = new List<string>();
            var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8)) {
                string line;
                while ((line = streamReader.ReadLine()) != null) {
                    list.Add(line);
                }
            }
            return list;
        }

        private static void OnChanged(object sender, FileSystemEventArgs e) {
            if (e.ChangeType != WatcherChangeTypes.Changed) {
                return;
            }
            Console.WriteLine($" [FileWatcher] Changed: {e.FullPath}\n");
        }

        private void StartSimulation() {
            var wh = new AutoResetEvent(false);
            Console.WriteLine(" [Simulation] Started..\n");

            // Instantiate two FileStreams, to avoid any conflicts in having two readers read from the same filestream
            var currentLineFs = new FileStream(LOG_FILE_PATH + INPUT_FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var nextLineFs = new FileStream(LOG_FILE_PATH + INPUT_FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            // Clear the LiveLog and create a writer
            File.WriteAllText(LOG_FILE_PATH + OUTPUT_FILE, String.Empty);

            using var writer = new StreamWriter(LOG_FILE_PATH + OUTPUT_FILE, true);
            using var currentReader = new StreamReader(currentLineFs);
            using var nextReader = new StreamReader(nextLineFs);
            nextReader.ReadLine();
            while (!currentReader.EndOfStream) {
                string currentLine = "";
                int timeUntilNextRead = 0;
                try {
                    currentLine = currentReader.ReadLine();
                    writer.WriteLine(currentLine);
                    writer.Flush();
                    Console.WriteLine(" [Simulation] Wrote to Log: \n" + currentLine + "\n");
                    if (currentLine != null && currentLine != "" && currentLine != String.Empty) {
                        string[] currentSplit = currentLine.Split(';');
                        string date = currentSplit[0];
                        string time = currentSplit[1];
                        string stationNo = currentSplit[2];
                        string msg = currentSplit[4];

                        string[] nextLine = nextReader.ReadLine().Split(';');
                        if (nextLine[1] != null) {
                            timeUntilNextRead = ((int)DateTime.Parse(nextLine[1]).Subtract(DateTime.Parse(time)).TotalMilliseconds);
                            Console.WriteLine(" [Simulation] Time until next read: " + timeUntilNextRead / 1000 + " seconds. \n");
                            wh.WaitOne(timeUntilNextRead != 0 ? timeUntilNextRead : 1000);
                        }
                    }
                } catch (OutOfMemoryException memEx) {
                    Console.Error.WriteLine(" [Simulation] Ran out of memory! Error-message: \n" + memEx.Message);
                } catch (IOException ioEx) {
                    Console.Error.WriteLine(" [Simulation] Input/Output-error! Error-message: \n" + ioEx.Message);
                }
            }
        }

        private string ConvertLogEntryToCurrentTime(string logEntry) {
            if (logEntry == null || logEntry.Equals(string.Empty)) return null;
            string[] logSplit = logEntry.Split(";");
            logSplit[0] = DateTime.Now.Date.ToString();
            logSplit[1] = DateTime.Now.TimeOfDay.ToString();
            return string.Join(";", logSplit);
        }

        public async void ParseNextLine() {

        }

        public async void PublishEvent() {

        }
    }
}
