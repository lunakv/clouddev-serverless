﻿using Azure.Messaging.EventHubs.Consumer;

namespace EvetHubConsumer
{
    internal class Program
    {
        private const string EventHubConnectionString = "Endpoint=sb://lunakvhub.servicebus.windows.net/;SharedAccessKeyName=lvhubkey;SharedAccessKey=fUg74CyRWRWiH0gW94yYt6xyUp7WErk0b+AEhJZo+io=;EntityPath=lvhub1";
        private const string EventHubName = "lvhub1"; // ie myeventhub1

        public static async Task Main(string[] args)
        {
            await MainAsync(args);
        }

        private static async Task MainAsync(string[] args)
        {
            var consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
            var consumer = new EventHubConsumerClient(consumerGroup, EventHubConnectionString, EventHubName);
            await using (consumer.ConfigureAwait(false))
            {
                try
                {
                    await ReadEventFromEventHubAsync(consumer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    await consumer.CloseAsync();
                }
            }
        }

        private static async Task ReadEventFromEventHubAsync(EventHubConsumerClient consumer)
        {
            string firstPartition = (await consumer.GetPartitionIdsAsync()).First();
            EventPosition startingPosition = EventPosition.Earliest;

            var options = new ReadEventOptions
            {
                TrackLastEnqueuedEventProperties = true
            };

            await foreach (var @event in consumer.ReadEventsFromPartitionAsync(firstPartition, startingPosition, options))
            {
                Console.WriteLine("Received new event: ");
                Console.WriteLine($"\tSequence number: {@event.Data.SequenceNumber}");
                Console.WriteLine($"\tPartition id: {@event.Partition.PartitionId}");
                Console.WriteLine($"\tBody: {@event.Data.EventBody}");
                Console.WriteLine();
            }
        }
    }
}