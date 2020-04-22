using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Earth
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");   
            
            //--- Setup

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            

            channel.QueueDeclare(queue: "a",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.QueueDeclare(queue: "b",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            channel.QueueDeclare(queue: "c",
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

            //channel.ExchangeDeclare("emergency", "header", true, false, null);

            //channel.QueueBind("a", "emergency", "red.*");
            //channel.QueueBind("b", "emergency", "*.report.*");
            //channel.QueueBind("c", "emergency", "*.critical");

            //--- Chat

            var message = " ";
            var format = "";

            while (message!=string.Empty)
            {
                Console.Write("Message : ");
                message = Console.ReadLine();

                Console.Write("Format : ");
                format = Console.ReadLine();


                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();



                var headers = new Dictionary<string, object>
                {
                    { "format", format },
                    { "time", DateTime.Now.ToShortTimeString() }
                };

                properties.Headers = headers;

                channel.BasicPublish("file.reports",
                     routingKey: string.Empty,
                     basicProperties: properties,
                     body: body);

                Console.WriteLine(" [Earth] Sent {0}", message);
            }
           


        }
    }
}
