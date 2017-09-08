using System;
using System.Linq;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace ReadDeviceToCloudCore
{
    class Program
    {
        static string connectionString = "{add endpoint and don't forget EntityPath=}";
        static EventHubClient eventHubClient;

        static void Main(string[] args)
        {
            Start();
            Console.Read();
        }
        static async void Start()
        {
            Console.WriteLine("Receive messages. Ctrl-C to exit.\n");

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString);

            var d2cPartitions = (await eventHubClient.GetRuntimeInformationAsync()).PartitionIds;

            CancellationTokenSource cts = new CancellationTokenSource();

            System.Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Exiting...");
            };

            var tasks = new List<Task>();
              foreach (string partition in d2cPartitions)
              {
                  tasks.Add(ReceiveMessagesFromDeviceAsync(partition));
              }

        //    tasks.Add(ReceiveMessagesFromDeviceAsync("3"));
            Task.WaitAll(tasks.ToArray());
        }

        static async Task<List<string>> ReceiveMessagesFromDeviceAsync(string partition)
        {
            List<string> messages = new List<string>();

            var eventHubReceiver = eventHubClient.CreateReceiver("consoleapp", partition, DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(30)));

            var eventDatas = await eventHubReceiver.ReceiveAsync(500);

            foreach (EventData data in eventDatas)
            {
                var array = data.Body.ToArray();

                string message = Encoding.UTF8.GetString(array);
                messages.Add(message);

                Console.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, message);
                System.Diagnostics.Debug.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, message);
            }

            return messages;
        }
    }
}