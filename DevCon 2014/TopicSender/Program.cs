using System;
using System.Threading;
using Common;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace TopicSender
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;

            var manager = NamespaceManager.Create();
			if ( !manager.TopicExists( "topic" ) )
			{
				manager.CreateTopic( "topic" );
			}

            var client = TopicClient.Create("topic");
            while (true)
            {
                var message = CreateNewMessage() ;
                client.Send(message);
                Console.Write(".");

                Thread.Sleep(100);
            }

        }

        private static BrokeredMessage CreateNewMessage()
        {
            var body = new Message();
            var message = new BrokeredMessage(body);
            message.Properties.Add("Value", body.Value);

            return message;
        }
    }
}
