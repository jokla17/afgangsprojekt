using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Globalization;

namespace API.RabbitMQ {
    public class RabbitService {
        public ConnectionFactory _factory;
        public IConnection _connection;
        public IModel _channel;

        public RabbitService() {
            _factory = new ConnectionFactory() { HostName = "localhost" };
        }

        private static string GetMessage(string[] args) {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }

        public void SendTask(string[] args) {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel()) {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(GetTimestamp() + " [x] Sent {0}", message);
            }

            Console.WriteLine(GetTimestamp() + " Press [enter] to exit.");
            Console.ReadLine();
        }

        public void Send() {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel()) {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(GetTimestamp() + " [x] Sent {0}", message);
            }

            Console.WriteLine(GetTimestamp() + " Press [enter] to exit.");
            Console.ReadLine();
        }

        public void Receive() {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel()) {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) => {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(GetTimestamp() + " [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "hello",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(GetTimestamp() + " Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        public void ReceiveTopic(string[]? bindingKeys) {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel()) {
                channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
                var queueName = channel.QueueDeclare().QueueName;
                if (bindingKeys == null || bindingKeys.Length < 1) {
                    Console.Error.WriteLine("RabbitMQ Receive Topic: Please provide a bindingKey");
                    return;
                }
                
                foreach(var bindingKey in bindingKeys) {
                    channel.QueueBind(queue: queueName,
                                      exchange: "topic_logs",
                                      routingKey: bindingKey);
                }

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) => {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",
                                        routingKey,
                                        message);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private string GetTimestamp() {
            DateTime now = DateTime.Now;

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            return now.ToString() + " > ";
        }
    }
}
