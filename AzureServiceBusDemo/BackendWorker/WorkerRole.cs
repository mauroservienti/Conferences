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

namespace BackendWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        QueueClient webFrontendClient;
        QueueClient inputClient;

        ManualResetEvent CompletedEvent = new ManualResetEvent( false );

        public override void Run()
        {
            Trace.WriteLine( "Starting processing of messages" );

            inputClient.OnMessage( ( receivedMessage ) =>
                 {
                     try
                     {
                         var body = receivedMessage.GetBody<MyMessage>();
                         receivedMessage.Complete();

                         var reply = new MyMessage() { Message = "Hey, there: " + body.Message + " -> " + DateTimeOffset.Now.Ticks };
                         webFrontendClient.Send( new BrokeredMessage( reply ) );
                     }
                     catch
                     {

                     }
                 } );

            CompletedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            var connectionString = CloudConfigurationManager.GetSetting( "Microsoft.ServiceBus.ConnectionString" );
            var namespaceManager = NamespaceManager.CreateFromConnectionString( connectionString );

            var inputQueueName = CloudConfigurationManager.GetSetting( "InputQueueName" );
            if( !namespaceManager.QueueExists( inputQueueName ) )
            {
                namespaceManager.CreateQueue( inputQueueName );
            }

            inputClient = QueueClient.CreateFromConnectionString( connectionString, inputQueueName );

            var webFrontendQueueName = CloudConfigurationManager.GetSetting( "WebFrontendQueueName" );
            if( !namespaceManager.QueueExists( webFrontendQueueName ) )
            {
                namespaceManager.CreateQueue( webFrontendQueueName );
            }

            webFrontendClient = QueueClient.CreateFromConnectionString( connectionString, webFrontendQueueName );

            return base.OnStart();
        }

        public override void OnStop()
        {
            inputClient.Close();
            CompletedEvent.Set();
            base.OnStop();
        }
    }
}
