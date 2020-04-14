using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Moon
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var listener = new RabbitListener();
            listener.Register();


        }

        public class RabbitListener
        {
            ConnectionFactory Factory { get; set; }
            IConnection Connection { get; set; }
            IModel Channel { get; set; }


            public RabbitListener()
            {
                Factory = new ConnectionFactory() { HostName = "localhost" };
                Connection = Factory.CreateConnection();
                Channel = Connection.CreateModel();


            }
            public void Register()
            {
                Channel.QueueDeclare(queue: "chatroom", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(Channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine(message);

                };
                Channel.BasicConsume(queue: "chatroom", autoAck: false, consumer: consumer);
            }

            public void Deregister()
            {
                this.Connection.Close();
            }


        }
    }
}
