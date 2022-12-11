using CompumatServer;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Server {
    public class Program {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";

        // NORTH lat: 55.31582463986073 (lng: 10.786626662287855)
        const double maxY = 55.31582463986073;

        // SOUTH lat: 55.31184251877076 (lng: 10.786940605771015)
        const double minY = 55.31184251877076;

        // EAST (lat: 55.31341572945177) lng: 10.790282336797844
        const double maxX = 10.790282336797844;

        // WEST (lat: 55.31357134607714) lng: 10.784520371697797
        const double minX = 10.784520371697797;

        private static Compumat[] compumats = new Compumat[] {
            new Compumat
            {
                Id = 01,
                Latitude = 0,
                Longitude = 0,
                Name = "test",
                Status = "ok",
                Type = Compumat.CompumatType.GATE
            },
        };


        private static string[] statusses = new string[] {
            "ok",
            "error",
            "offline"
        };

        private static void GenerateCompumats(int amount) {
            compumats = new Compumat[amount];
            const string chars = "ABCDEFGIJKLMOPQRSTUVXYZ0123456789";
            Random random = new Random();
            for (int i = 0; i < amount; i++) {
                compumats[i] = new Compumat {
                    Id = i,
                    Latitude = GetPseudoDoubleWithinRange(minY, maxY),
                    Longitude = GetPseudoDoubleWithinRange(minX, maxX),
                    Name = new string(Enumerable.Repeat(chars, 8)
                        .Select(s => s[random.Next(s.Length)]).ToArray()),
                    Status = statusses[random.Next(0, 3)],
                    Type = (Compumat.CompumatType)random.Next(1, 3)
                };
            }

            Console.WriteLine("Generated " + amount + " devices: ");
            foreach (Compumat compumat in compumats) {
                Console.WriteLine(compumat.Name);
            }
        }

        public static double GetPseudoDoubleWithinRange(double lowerBound, double upperBound) {
            var random = new Random();
            var rDouble = random.NextDouble();
            var rRangeDouble = rDouble * (upperBound - lowerBound) + lowerBound;
            return rRangeDouble;
        }

        static void Main(string[] args) {
            MonitorLogService logService = new MonitorLogService();
            logService.Start();
            string input = null;
            while(input == null) {
                input = Console.ReadLine();
                if (input != null) break;
            }
            /* Is able to emit messages through RabbitMQ
            RabbitService rs = new RabbitService();
            //rs.ReceiveTask();
            string input1 = "";
            string input2 = "";
            Console.WriteLine("Now emitting every time you enter a message. Type 'q' to exit.");
            while (input1 != "q" && input2 != "q") {
                Console.WriteLine("Please enter the routing-key: ");
                string routingKey = Console.ReadLine();
                input1 = routingKey;
                if (routingKey == "q") break;
                Console.WriteLine("Please enter the message: ");
                string message = Console.ReadLine();
                input2 = message;
                if (message == "q") break;

                rs.Emit(routingKey, message); 
            }*/
        }

        private static async void RandomizeDevices() {
            foreach (Compumat device in compumats) {
                Random r = new Random();
                int nextStatusIndex = r.Next(0, 3);
                device.Status = statusses[nextStatusIndex];
                Console.WriteLine("Updated compumatstatus to " + device.Status);
            }
        }

    }
    public static class XmlHelper {
        public static XElement ToXElement<T>(this object obj) {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            XDocument doc = new XDocument();
            using (XmlWriter xw = doc.CreateWriter()) {
                ser.Serialize(xw, obj, namespaces);
                xw.Close();
            }
            return doc.Root;
        }
    }

    /* PUT THIS IN MAIN() TO GENERATE COMPUMATS
            GenerateCompumats(100);
            IPAddress ipAddr = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(ipAddr, PORT_NO);
            Console.WriteLine("Listening...");
            listener.Start();

            while (true) {
                RandomizeDevices();

                TcpClient client = listener.AcceptTcpClient();

                NetworkStream nwStream = client.GetStream();

                byte[] bytesToSend = new byte[client.ReceiveBufferSize];

                int bytesRead = nwStream.Read(bytesToSend, 0, bytesToSend.Length);

                string dataReceived = Encoding.ASCII.GetString(bytesToSend, 0, bytesRead);
                Console.WriteLine("Received : " + dataReceived);

                if(dataReceived == "poll") {
                    XElement result = new XElement("Compumats");
                    foreach (Compumat compumat in compumats) {
                        XElement el = XmlHelper.ToXElement<Compumat>(compumat);
                        result.Add(el);
                    }
                    bytesToSend = ASCIIEncoding.ASCII.GetBytes(result.ToString());
                    Console.WriteLine("Sending back : " + result);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                } else {
                    Console.WriteLine("Sending back : " + dataReceived);
                    nwStream.Write(bytesToSend, 0, bytesRead);
                }


                client.Close();
            }
            listener.Stop();
            Console.ReadLine();
            */
}