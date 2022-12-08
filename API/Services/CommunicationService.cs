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
                    List<Compumat> compumats = ParseCompumat(recTxt);

                    Thread.Sleep(DELAY_MS);
                    if (token.IsCancellationRequested) {
                        break;
                    }
                }
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            
        }

        public void StopPolling() {
            this._cts.Cancel();
            this._cts = new CancellationTokenSource();
        }

        private List<Compumat> ParseCompumat(string compumatList) {
            XElement el = XmlHelper.ToXElement<string>(compumatList);
            XDocument xDoc = XDocument.Parse(compumatList);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Compumats));
            Compumats compumats = (Compumats)xmlSerializer.Deserialize(xDoc.CreateReader());
            return compumats.compumats;
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
