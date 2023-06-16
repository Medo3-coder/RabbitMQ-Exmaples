using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ExToExDemo
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


            //create exchanges 
            channel.ExchangeDeclare("exchange1", "direct", true, false, null);
            channel.ExchangeDeclare("exchange2", "direct", true, false, null);

            //create queues 
            channel.QueueDeclare("queue1", true, false, false, null);
            channel.QueueDeclare("queue2", true, false, false, null);

            //binding queue
            channel.QueueBind("queue1", "exchange1", "key1", null);
            channel.QueueBind("queue2", "exchange2", "key2", null);

            //binding exchange to exchange
            channel.ExchangeBind("exchange2", "exchange1" , "key2");


            //publish message 
            channel.BasicPublish("exchange1", "key1", null, Encoding.UTF8.GetBytes("message with routing key1"));
            channel.BasicPublish("exchange1", "key2", null, Encoding.UTF8.GetBytes("message with routing key2"));








        }
    }
}
