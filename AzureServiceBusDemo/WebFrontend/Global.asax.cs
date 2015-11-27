using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using SharedMessages;

namespace WebFrontend
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static List<MyMessage> Inbox { get; private set; }
        public static QueueClient BackendWorkerClient { get; private set; }
        QueueClient inputClient;

        static WebApiApplication()
        {
            Inbox = new List<MyMessage>();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure( WebApiConfig.Register );
            FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
            RouteConfig.RegisterRoutes( RouteTable.Routes );
            BundleConfig.RegisterBundles( BundleTable.Bundles );

            var connectionString = CloudConfigurationManager.GetSetting( "Microsoft.ServiceBus.ConnectionString" );
            var namespaceManager = NamespaceManager.CreateFromConnectionString( connectionString );

            var qName = CloudConfigurationManager.GetSetting( "InputQueueName" );
            if( !namespaceManager.QueueExists( qName ) )
            {
                namespaceManager.CreateQueue( qName );
            }

            inputClient = QueueClient.CreateFromConnectionString( connectionString, qName );


            var backendWorkerQueueName = CloudConfigurationManager.GetSetting( "BackendWorkerQueueName" );
            if( !namespaceManager.QueueExists( backendWorkerQueueName ) )
            {
                namespaceManager.CreateQueue( backendWorkerQueueName );
            }

            BackendWorkerClient = QueueClient.CreateFromConnectionString( connectionString, backendWorkerQueueName );

            inputClient.OnMessage( ( receivedMessage ) =>
            {
                try
                {
                    var body = receivedMessage.GetBody<MyMessage>();
                    Inbox.Insert( 0, body );
                    receivedMessage.Complete();
                }
                catch
                {

                }
            } );
        }
    }
}
