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

            //direct,fanout,topic,header
            channel.ExchangeDeclare("contact", "direct", false, false, null);
            channel.ExchangeDeclare("alarms", "fanout", false, false, null);
            channel.ExchangeDeclare("subject", "topic", false, false, null);
            channel.ExchangeDeclare("format", "headers", false, false, null);


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

            var headerBind1 = new Dictionary<string, object>();
            headerBind1.Add("x-match", "any");
            headerBind1.Add("format", "bmp");
            channel.QueueBind("a", "format", "", headerBind1);


            var headerBind2 = new Dictionary<string, object>();
            headerBind2.Add("x-match", "any");
            headerBind2.Add("format", "mkv");
            channel.QueueBind("b", "format", "", headerBind2);

            channel.QueueBind("c", "format", "");


            //--- Chat

            var message = " ";
            var format = "";
            while (message != string.Empty)
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
                    { "time", DateTime.Now.ToShortTimeString()
                    }
                };

                properties.Headers = headers;

                channel.BasicPublish(exchange: "format",
                     routingKey: format,
                     basicProperties: properties,
                     body: body);

                Console.WriteLine(" [Earth] Sent {0}", message);
            }



        }
    }
}

