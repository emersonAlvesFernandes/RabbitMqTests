using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqAppReceiver
{
    class Program
    {
        public static void Main()
        {
            //var factory = new ConnectionFactory() { HostName = "localhost" };

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "testExchange", type: "direct", durable: true);

                //var queueName = channel.QueueDeclare("testQueue", durable: true).QueueName;
                var queueName = "testQueue";

                channel.QueueBind(queue: queueName,
                                  exchange: "testExchange",
                                  routingKey: "testRoutingKey");

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
