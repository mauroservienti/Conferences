using System;
using System.Threading;
using Common;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace ProcessReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;

            var manager = NamespaceManager.Create();
			if ( manager.SubscriptionExists( "topic", "process" ) )
			{
				manager.DeleteSubscription( "topic", "process" );
			}

			if ( !manager.SubscriptionExists( "topic", "process" ) )
			{
				manager.CreateSubscription( "topic", "process", new SqlFilter( "Value < 500" ) );
			}

            var client = SubscriptionClient.Create("topic", "process");
            
            while (true)
            {
                var message = client.Receive();

                var body = message.GetBody<Message>();
				Console.WriteLine( "{0} - {1}", body.Id, body.Value );
                Console.WriteLine("{0} - {1}", message.Properties["Value"], body.Value);

                message.Complete();

                Thread.Sleep(100);
            }
        }
    }
}
