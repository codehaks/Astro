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
            

            channel.QueueDeclare(queue: "inbox",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            //--- Chat

            var message = " ";

            while (message!=string.Empty)
            {
                Console.WriteLine("Message : ");
                message = Console.ReadLine();
     
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                     routingKey: "inbox",
                     basicProperties: null,
                     body: body);

                Console.WriteLine(" [Earth] Sent {0}", message);
            }
           


        }
    }
}
