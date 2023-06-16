using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace AlternateDemo
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
            channel.ExchangeDeclare("ex.fanout", "fanout", true, false, null);
            channel.ExchangeDeclare("ex.direct", "direct", true, false,
                new Dictionary<string, object>()
                {
                    {"alternate-exchange" , "ex.fanout" }
                });

            //create queues
            channel.QueueDeclare("my.queue1", true, false, false, null);
            channel.QueueDeclare("my.queue2", true, false, false, null);
            channel.QueueDeclare("my.unrouted", true, false, false, null);

            //bind 
            channel.QueueBind("my.queue1", "ex.direct", "video");
            channel.QueueBind("my.queue2", "ex.direct", "image");
            channel.QueueBind("my.unrouted", "ex.fanout", "");

            //publish message 
            channel.BasicPublish("ex.direct", "video", null, Encoding.UTF8.GetBytes("message with routing key video"));
            channel.BasicPublish("ex.direct", "text", null, Encoding.UTF8.GetBytes("message with routing key text"));





        }
    }
}
