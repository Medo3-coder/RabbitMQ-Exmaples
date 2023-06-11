using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace FanoutPublisher1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConnection connection;
            IModel channel;

            //create connection to rabbitMQ
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

            //declare queue 1
            channel.QueueDeclare("my.queue1", true, false, false, null);

            //declare queue 2
            channel.QueueDeclare("my.queue2", true, false, false, null);


            //bind this queues
            channel.QueueBind("my.queue1", "ex.fanout", "");
            channel.QueueBind("my.queue2", "ex.fanout", "");

            //publish message to exchang
            channel.BasicPublish("ex.fanout", "", null, Encoding.UTF8.GetBytes("Message 1"));
            channel.BasicPublish("ex.fanout", "", null, Encoding.UTF8.GetBytes("Message 2"));


            channel.QueueDelete("my.queue1");
            channel.QueueDelete("my.queue2");
            channel.ExchangeDelete("ex.fanout");

            //close connection  and channel 
            channel.Close();
            connection.Close();



        }
    }
}



