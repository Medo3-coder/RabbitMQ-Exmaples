using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WorkerDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the name for this worker:");
            string workerName = Console.ReadLine();

            //create connection 
            ConnectionFactory factory = new ConnectionFactory();
            factory.VirtualHost = "/";
            factory.HostName = "localhost";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            //to get message automatically u need to subscribe to queue so u need to make consumer 

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                string message = System.Text.Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{workerName}] Message:{message}");
            };

            //subscribe to queue 
            var consumerTag = channel.BasicConsume("my.queue1" , true , consumer);
            Console.WriteLine("Waiting for messages. Press a key to exit.");
            Console.ReadKey();

            channel.Close();
            connection.Close();

        }
    }
}
