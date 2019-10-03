using Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RabbitMQ.Producer
{
    class Program
    {
        private const string QueueName = "WorkerQueue_Queue";
        static void Main(string[] args)
        {

            var objs = new List<DMFObject>();

            for (int i = 1; i <= 1000; i++)
            {
                objs.Add(new DMFObject() { FirstName = $"John Doe {i}" });
            }


            var client = new RabbitMQClient();

            client.CreateConnection();
            var watch = System.Diagnostics.Stopwatch.StartNew();

            objs.ForEach(x => client.SendMessage(x));

            Task.WaitAll();
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);


            Console.ReadLine();
            client.Close();
        }
    }
}
