using System;
using System.Threading;
using Common;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace MonitoringReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;

            var manager = NamespaceManager.Create();
            if (!manager.TopicExists("topic"))
                manager.CreateTopic("topic");


            var client = SubscriptionClient.Create("topic", "monitoring");
            while (true)
            {
                var message = client.Receive();
                var body = message.GetBody<Message>();
                Console.WriteLine("{0} - {1}", body.Id, body.Value);

                message.Complete();

                Thread.Sleep(2000);
            }
        }
    }
}
