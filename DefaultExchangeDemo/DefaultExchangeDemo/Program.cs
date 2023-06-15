using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace DefaultExchangeDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConnection connection;
            IModel channel; 

            //create connection 
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            //create queues
            channel.QueueDeclare("my.queue1", true, false, false, null);
            channel.QueueDeclare("my.queue2", true, false, false, null);

            //Publish Message 
            //to use default exchange u must leave exchange name empty 
            channel.BasicPublish("", "my.queue1", null, Encoding.UTF8.GetBytes("message with routing my.queue1"));
            channel.BasicPublish("", "my.queue2", null, Encoding.UTF8.GetBytes("message with routing my.queue2"));




        }
    }
}
