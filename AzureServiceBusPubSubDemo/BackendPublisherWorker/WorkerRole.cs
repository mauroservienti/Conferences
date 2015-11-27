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

namespace BackendPublisherWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        QueueClient webFrontendClient;
        QueueClient inputClient;
        TopicClient topicClient;

        ManualResetEvent CompletedEvent = new ManualResetEvent( false );

        public override void Run()
        {
            Trace.WriteLine( "Starting processing of messages" );

            var options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(1)
            };

            inputClient.OnMessage( ( receivedMessage ) =>
            {
                try
                {
                    var body = receivedMessage.GetBody<MyMessage>();

                    var reply = new MyMessage() { Message = "Hey, there message received by 'BackendPublisherWorker': " + body.Message + " -> " + DateTimeOffset.Now.Ticks };
                    webFrontendClient.Send( new BrokeredMessage( reply ) );

                    var @event = new MyMessageReceived() { Message = "Hey, there: " + body.Message + " -> " + DateTimeOffset.Now.Ticks };
                    topicClient.Send( new BrokeredMessage( @event ) );

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
            var inputQueueName = CloudConfigurationManager.GetSetting( "InputQueueName" );
            var topicName = CloudConfigurationManager.GetSetting( "PublisherTopicName" );
            var webFrontendQueueName = CloudConfigurationManager.GetSetting( "WebFrontendQueueName" );

            NamespaceManager.CreateFromConnectionString( connectionString )
                .CreateQueueIfNotExists( inputQueueName )
                .CreateQueueIfNotExists( webFrontendQueueName )
                .CreateTopicIfNotExists( topicName );


            inputClient = QueueClient.CreateFromConnectionString( connectionString, inputQueueName );
            webFrontendClient = QueueClient.CreateFromConnectionString( connectionString, webFrontendQueueName );
            topicClient = TopicClient.CreateFromConnectionString( connectionString, topicName );

            return base.OnStart();
        }

        public override void OnStop()
        {
            inputClient.Close();
            webFrontendClient.Close();
            topicClient.Close();

            CompletedEvent.Set();

            base.OnStop();
        }
    }
}
