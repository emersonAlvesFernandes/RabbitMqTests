using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace RabbitMqApp.Emiter
{
    class Program
    {
        private static IConfiguration _configuration;
        public static void Main(string[] args)
        {
            var rabbitMQConfigurations = new RabbitMQConfigurations();
            new ConfigureFromConfigurationOptions<RabbitMQConfigurations>(
               _configuration.GetSection("RabbitMQConfigurations"))
                   .Configure(rabbitMQConfigurations);

            var factory = new ConnectionFactory()
            {
                HostName = "localhost", //rabbitMQConfigurations.HostName,
                Port = 5672, //rabbitMQConfigurations.Port,
                UserName = "guest", //rabbitMQConfigurations.UserName,
                Password = "guest", //rabbitMQConfigurations.Password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);

            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
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
