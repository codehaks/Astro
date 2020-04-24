using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
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
            channel.ExchangeDeclare("contact", "direct", false,false, null);
            channel.ExchangeDeclare("alarms", "fanout", false, false, null);
            channel.ExchangeDeclare("subject", "topic", false, false, null);


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

            //channel.QueueBind("a", "contact", "blue");
            //channel.QueueBind("b", "contact", "blue");
            //channel.QueueBind("c", "contact", "red");

            //channel.QueueBind("a", "contact", "black");
            //channel.QueueBind("b", "contact", "black");
            //channel.QueueBind("c", "contact", "black");

            //channel.QueueBind("a", "alarms", "");
            //channel.QueueBind("b", "alarms", "");
            //channel.QueueBind("c", "alarms", "");

            channel.QueueBind("a", "subject", "*.orders.*");
            channel.QueueBind("b", "subject", "high.*");
            channel.QueueBind("c", "subject", "logging.#");


            //--- Chat

            var message = " ";
            var key = "";
            while (message!=string.Empty)
            {
                Console.Write("Message : ");
                message = Console.ReadLine();

                Console.Write("Key : ");
                key = Console.ReadLine();

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "subject",
                     routingKey: key,
                     basicProperties: null,
                     body: body);

                Console.WriteLine(" [Earth] Sent {0}", message);
            }
           


        }
    }
}
