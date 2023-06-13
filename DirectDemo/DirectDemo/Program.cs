using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace DirectDemo
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

            //create exchange 
            channel.ExchangeDeclare("ex.direct" , "direct" , true, false , null);

            //create queues
            channel.QueueDeclare("my.infos" , true , false ,false , null);
            channel.QueueDeclare("my.warnings", true, false, false, null);
            channel.QueueDeclare("my.errors", true, false, false, null);

            //binding to exchange 
            channel.QueueBind("my.infos", "ex.direct" , "info");
            channel.QueueBind("my.warnings", "ex.direct", "warning");
            channel.QueueBind("my.errors", "ex.direct", "error");

            //Publish Message 
            channel.BasicPublish("ex.direct", "info", null , Encoding.UTF8.GetBytes("message with routing key info."));
            channel.BasicPublish("ex.direct", "warning", null, Encoding.UTF8.GetBytes("message with routing key warning."));
            channel.BasicPublish("ex.direct", "error", null, Encoding.UTF8.GetBytes("message with routing key error."));






        }
    }
}
