using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace HeadersDemo
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
            channel.ExchangeDeclare("ex.headers", "headers", true, false, null);

            //create queues
            channel.QueueDeclare("my.queue1", true, false, false, null);
            channel.QueueDeclare("my.queue2", true, false, false, null);

            //binding to exchange
            channel.QueueBind("my.queue1", "ex.headers", "",
                new Dictionary<string, object>()
                {
                    {"x-match" , "all"},
                    {"job" , "convert" },
                    {"format" , "jpeg" }
                });


            channel.QueueBind("my.queue1", "ex.headers", "",
                new Dictionary<string, object>()
                {
                    {"x-match" , "all"},
                    {"job" , "convert" },
                    {"format" , "jpeg" }
                });


            channel.QueueBind("my.queue2", "ex.headers", "",
              new Dictionary<string, object>()
              {
                    {"x-match" , "any"},
                    {"job" , "convert" },
                    {"format" , "jpeg" }
              });

            //to provide header with message value create base properties
            IBasicProperties props = channel.CreateBasicProperties();
            props.Headers = new Dictionary<string, object>();
            props.Headers.Add("job", "convert");
            props.Headers.Add("format", "jpeg");

            // publish message
            channel.BasicPublish("ex.headers" , "" , props , Encoding.UTF8.GetBytes("messge 1"));


            //second message 


            props = channel.CreateBasicProperties();
            props.Headers = new Dictionary<string, object>();
            props.Headers.Add("job", "convert");
            props.Headers.Add("format", "bmp");

            // publish message
            channel.BasicPublish("ex.headers", "", props, Encoding.UTF8.GetBytes("messge 2"));

        }
    }
}
