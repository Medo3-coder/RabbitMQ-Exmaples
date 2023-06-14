using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace TopicDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConnection connection;
            IModel channel;

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";
            connection = factory.CreateConnection();
            channel = connection.CreateModel();


            //create exchange 
            channel.ExchangeDeclare("ex.topic", "topic", true, false, null);

            //create queues
            channel.QueueDeclare("my.queue1", true, false, false, null);
            channel.QueueDeclare("my.queue2", true, false, false, null);
            channel.QueueDeclare("my.queue3", true, false, false, null);

            //binding these queues to topic exchange
            channel.QueueBind("my.queue1", "ex.topic", "*.image.*");
            channel.QueueBind("my.queue2", "ex.topic", "#.image");
            channel.QueueBind("my.queue3", "ex.topic", "image.#");

            //Publish Message
            channel.BasicPublish("ex.topic", "convert.image.bmp", null, Encoding.UTF8.GetBytes("Routing Key IS convert.image.bmp"));
            channel.BasicPublish("ex.topic", "convert.bitmap.image", null, Encoding.UTF8.GetBytes("Routing Key IS convert.bitmap.image"));
            channel.BasicPublish("ex.topic", "image.bitmap.32bit", null, Encoding.UTF8.GetBytes("Routing Key IS image.bitmap.32bit"));







        }
    }
}
