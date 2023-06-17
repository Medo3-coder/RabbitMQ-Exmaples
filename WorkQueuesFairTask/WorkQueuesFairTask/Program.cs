using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WorkQueuesFairTask
{
    /*
     First we will see what happens if we distribute equal number of tasks to each worker.
    When the duration of each task differs too much.
    We will send the task duration as the message and the worker will wait that amount of time to simulate
     */
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the name for this worker:");
            string workerName = Console.ReadLine();

            //create connection 
            ConnectionFactory factory = new ConnectionFactory();
            factory.VirtualHost = "/";
            factory.HostName = "localhost";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            //  we will tell the rabbitMq thank you not to send in other task to the worker if it already received
            //a task and has not sent an acknowledgement for it yet which means it is still processing that test to channel to do this use channel base cos method


            //Basically prefetch in both cases means (maximum allowed) number of unacknowledged messages that will be sent to the worker,

            channel.BasicQos(0, 1, false);

            //to get message automatically u need to subscribe to queue so u need to make consumer 

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                string message = System.Text.Encoding.UTF8.GetString(body);
                int durationInSeconds = Int32.Parse(message);
                Console.Write("[" + workerName + "] Task Started. Duration: " + durationInSeconds);
                Thread.Sleep(durationInSeconds * 1000);
                Console.WriteLine("finished");
                channel.BasicAck(e.DeliveryTag, false);


                // When a consumer (subscription) is registered, messages will be delivered (pushed) by RabbitMQ using the basic. deliver method.
                // * The method carries a delivery tag, 
                // which uniquely identifies the delivery on a channel. Delivery tags are therefore scoped per channel.

            };


       


            //subscribe to queue 
            var consumerTag = channel.BasicConsume("my.queue1", false, consumer);
            Console.WriteLine("Waiting for messages. Press a key to exit.");
            Console.ReadKey();

            channel.Close();
            connection.Close();

        }
    }
}
