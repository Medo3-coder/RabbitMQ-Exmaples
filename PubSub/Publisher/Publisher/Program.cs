using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Publisher
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

            while (true)
            {
                Console.Write("Enter Message:");
                string message = Console.ReadLine();
                if(message == "exit") break;

                channel.BasicPublish("ex.fanout" , "" , null , Encoding.UTF8.GetBytes(message));

            }

            channel.Close();
            connection.Close();

        }
    }
}
