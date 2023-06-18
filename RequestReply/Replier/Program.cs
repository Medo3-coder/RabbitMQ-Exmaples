using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Replier
{
    internal class Program
    {

        //replied application will also be a publisher and subscriber it will subscribe to the request queue and
        //get request from the requesters process the request and publish the responses to the responses queue
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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                string requests = Encoding.UTF8.GetString(body);
                Console.WriteLine("request recived: " +  requests);


                //response 
                string response = "response for " + requests;
                channel.BasicPublish("" , "responses" , null , Encoding.UTF8.GetBytes(response));
            };

            channel.BasicConsume("requests" , true , consumer);
            Console.WriteLine("Press a key to exit.");
            Console.ReadKey();

            connection.Close();
            channel.Close();

        }
    }
}
