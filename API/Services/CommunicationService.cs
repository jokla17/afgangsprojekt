using API.Hubs;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace API.Services {
    public class CommunicationService {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";
        const int DELAY_MS = 2000;
        TcpClient tcpClient;
        CancellationTokenSource _cts;
        CompumatHub hub;

        public CommunicationService() { 
            this._cts = new CancellationTokenSource();
        }

        public string SendObject(Dictionary<string,string> message) {
            tcpClient = new TcpClient(SERVER_IP, PORT_NO);

            NetworkStream nwStream = tcpClient.GetStream();

            XElement el = new XElement("root",
                message.Select(kc => new XElement(kc.Key,kc.Value)));

            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(el.ToString());

            Console.WriteLine("Sending : " + message);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            byte[] bytesToRead = new byte[tcpClient.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, tcpClient.ReceiveBufferSize);
            string receivedTxt = "Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);

            tcpClient.Close();

            return receivedTxt;
        }

        public string SendMessage(string message) {
            tcpClient = new TcpClient(SERVER_IP, PORT_NO);

            NetworkStream nwStream = tcpClient.GetStream();

            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(message);

            Console.WriteLine("Sending : " + message);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            byte[] bytesToRead = new byte[tcpClient.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, tcpClient.ReceiveBufferSize);
            string receivedTxt = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
            Console.WriteLine("Received : " + receivedTxt);

            tcpClient.Close();

            return receivedTxt;
        }

        public async void PollServer() {
            var token = this._cts.Token;
            await Task.Factory.StartNew(async () => {
                while (true) {
                    string recTxt = this.SendMessage("poll");

                    Debug.WriteLine(recTxt);
                    List<Compumat> compumats = ParseCompumats(recTxt);

                    Thread.Sleep(DELAY_MS);
                    if (token.IsCancellationRequested) {
                        break;
                    }
                }
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public async Task<List<LogEntry>> GetLog() {
            string recTxt = this.SendMessage("GetLog");

            Debug.WriteLine(recTxt);
            List<LogEntry> log = ParseLogEntries(recTxt);
            return log;
        }

        public async Task<List<LogEntry>> GetCompumatLog(string compumatId) {
            string recTxt = this.SendMessage($"GetCompumatLog:{compumatId}");

            Debug.WriteLine(recTxt);
            List<LogEntry> log = ParseLogEntries(recTxt);
            List<LogEntry> result = new List<LogEntry>();
            foreach (LogEntry entry in log) {
                if(string.Compare(entry.CompumatId, compumatId, true) == 0) {
                    result.Add(entry);
                }
            }
            return result;
        }

        public void StopPolling() {
            this._cts.Cancel();
            this._cts = new CancellationTokenSource();
        }

        private List<Compumat> ParseCompumats(string compumatList) {
            XElement el = XmlHelper.ToXElement<string>(compumatList);
            XDocument xDoc = XDocument.Parse(compumatList);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Compumats));
            Compumats compumats = (Compumats)xmlSerializer.Deserialize(xDoc.CreateReader());
            return compumats.compumats;
        }

        private List<LogEntry> ParseLogEntries(string logString) {
            XElement el = XmlHelper.ToXElement<string>(logString);
            XDocument xDoc = XDocument.Parse(logString);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Log));
            Log log = (Log)xmlSerializer.Deserialize(xDoc.CreateReader());
            return log.log;
        }

    }

    public static class XmlHelper {
        public static T FromXElement<T>(this XElement xElement) {
            var xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(""));
            return (T)xmlSerializer.Deserialize(xElement.CreateReader());
        }

        public static XElement ToXElement<T>(this object obj) {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            XDocument doc = new XDocument();
            using (XmlWriter xw = doc.CreateWriter()) {
                ser.Serialize(xw, obj);
                xw.Close();
            }
            return doc.Root;
        }
    }
}
