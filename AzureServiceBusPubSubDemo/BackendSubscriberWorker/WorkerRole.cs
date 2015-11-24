using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using SharedMessages;

namespace BackendSubscriberWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        SubscriptionClient subscriptionClient;
        QueueClient webFrontendClient;

        ManualResetEvent CompletedEvent = new ManualResetEvent( false );

        public override void Run()
        {
            Trace.WriteLine( "Starting processing of messages" );

            var options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes( 1 )
            };

            subscriptionClient.OnMessage( ( receivedMessage ) =>
            {
                try
                {
                    var body = receivedMessage.GetBody<MyMessageReceived>();

                    var reply = new MyMessage() { Message = "Event received by 'BackendSubscriberWorker': " + body.Message + " -> " + DateTimeOffset.Now.Ticks };
                    webFrontendClient.Send( new BrokeredMessage( reply ) );

                    receivedMessage.Complete();
                }
                catch
                {
                    receivedMessage.Abandon();
                }
            }, options );

            CompletedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            var connectionString = CloudConfigurationManager.GetSetting( "Microsoft.ServiceBus.ConnectionString" );
            var topicName = CloudConfigurationManager.GetSetting( "PublisherTopicName" );
            var webFrontendQueueName = CloudConfigurationManager.GetSetting( "WebFrontendQueueName" );

            NamespaceManager.CreateFromConnectionString( connectionString )
                .CreateSubscriptionIfNotExists( topicName, "BackendSubscriberWorker-Subscription" )
                .CreateQueueIfNotExists( webFrontendQueueName );


            subscriptionClient = SubscriptionClient.CreateFromConnectionString( 
                connectionString, 
                topicName,
                "BackendSubscriberWorker-Subscription" );

            webFrontendClient = QueueClient.CreateFromConnectionString( connectionString, webFrontendQueueName );

            return base.OnStart();
        }

        public override void OnStop()
        {
            subscriptionClient.Close();
            webFrontendClient.Close();

            CompletedEvent.Set();
            base.OnStop();
        }
    }
}
