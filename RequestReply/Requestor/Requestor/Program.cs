using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Requestor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";

            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            //first it will create request and sent or pushlish them to request queue and it will recive response 

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Response received:" + message);

            };

            //sub to response queue 

            channel.BasicConsume("responses", true , consumer);

             //reading meesage from console and publish it

            while (true)
            {
                Console.Write("Enter Your Request:");
                string request = Console.ReadLine();

                if (request == "exit") break;
                //publish this request to request queue 
                channel.BasicPublish("" , "requests" , null, Encoding.UTF8.GetBytes(request));
            }

            channel.Close();
            connection.Close();
        }
    }
}
