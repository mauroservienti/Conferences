using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using Common;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;
            
            var manager = NamespaceManager.Create();
			if ( !manager.QueueExists( "queue" ) )
			{
				manager.CreateQueue( "queue" );
			}

            var queue = QueueClient.Create("queue");
            Console.WriteLine("RECEIVER");
            while (true)
            {
                var message = queue.Receive();
                var body = message.GetBody<Message>();

                Console.WriteLine("Received message w/ Id: {0}", body.Id);
                
                message.Complete();

                Thread.Sleep(200);
            }
        }

    }
}
