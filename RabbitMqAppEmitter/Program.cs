using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;

namespace RabbitMqAppEmitter
{
    class Program
    {
        private static IConfiguration _configuration;
        public static void Main(string[] args)
        {
            //var rabbitMQConfigurations = new RabbitMQConfigurations();
            // new ConfigureFromConfigurationOptions<RabbitMQConfigurations>(
            //    _configuration.GetSection("RabbitMQConfigurations"))
            //        .Configure(rabbitMQConfigurations);

            var factory = new ConnectionFactory()
            {
                HostName = "localhost", //rabbitMQConfigurations.HostName,
                Port = 5672,            //rabbitMQConfigurations.Port,
                UserName = "guest",     //rabbitMQConfigurations.UserName,
                Password = "guest",     //rabbitMQConfigurations.Password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "testExchange", type: "direct", durable: true);

                // var message = GetMessage(args);
                var message = "Mensagem para testRoutingKey";

                var body = Encoding.UTF8.GetBytes(message);

                for (int i = 0; i <= 5; i++)
                {
                    Thread.Sleep(200);

                    channel.BasicPublish(exchange: "testExchange",
                                     routingKey: "testRoutingKey",
                                     basicProperties: null,
                                     body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }



            }

            //***********************/

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "testExchange", type: "direct", durable: true);

                // var message = GetMessage(args);
                var message = "Mensagem para testTwoRoutingKey";

                var body = Encoding.UTF8.GetBytes(message);

                for (int i = 0; i <= 5; i++)
                {
                    Thread.Sleep(200);

                    channel.BasicPublish(exchange: "testExchange",
                                     routingKey: "testTwoRoutingKey",
                                     basicProperties: null,
                                     body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }                
            }
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0)
                   ? string.Join(" ", args)
                   : "info: Hello World!");
        }

        private static void Consumer_Received(
           object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine(Environment.NewLine +
                "[Nova mensagem recebida] " + message);

            //List<Cotacao> cotacoes;
            //PaginaCotacoes pagina =
            //    new PaginaCotacoes(_seleniumConfigurations);
            //try
            //{
            //    Console.WriteLine("Iniciando extração dos dados...");
            //    pagina.CarregarPagina();
            //    cotacoes = pagina.ObterCotacoes();
            //    Console.WriteLine("Dados extraídos com sucesso!");

            //    new CotacoesDAO(_configuration.GetConnectionString("TestesRabbitMQ"))
            //        .CarregarDados(cotacoes);
            //    Console.WriteLine("Carga dos dados efetuada com sucesso!");
            //}
            //finally
            //{
            //    pagina.Fechar();
            //}
        }
    }
}
