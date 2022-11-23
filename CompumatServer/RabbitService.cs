using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Globalization;

namespace CompumatServer {
    class RabbitService {
        public ConnectionFactory _factory;

        public RabbitService() {
            _factory = new ConnectionFactory() { HostName = "localhost" };
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
                consumer.Received += (model, ea) =>
                {
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

        public void ReceiveTask() {
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

                    int dots = message.Split('.').Length - 1;
                    Thread.Sleep(dots * 1000);

                    Console.WriteLine(GetTimestamp() + " [x] Done");

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                channel.BasicConsume(queue: "hello",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(GetTimestamp() + " Press [enter] to exit.");
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
