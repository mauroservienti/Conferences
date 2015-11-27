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
        public static QueueClient BackendPublisherWorkerClient { get; private set; }
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
            var inputQueueName = CloudConfigurationManager.GetSetting( "InputQueueName" );
            var backendWorkerQueueName = CloudConfigurationManager.GetSetting( "BackendPublisherWorkerQueueName" );

            NamespaceManager.CreateFromConnectionString( connectionString )
                .CreateQueueIfNotExists( inputQueueName )
                .CreateQueueIfNotExists( backendWorkerQueueName );

            
            inputClient = QueueClient.CreateFromConnectionString( connectionString, inputQueueName );
            BackendPublisherWorkerClient = QueueClient.CreateFromConnectionString( connectionString, backendWorkerQueueName );

            var options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes( 1 )
            };

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
                    receivedMessage.Abandon();
                }
            }, options );
        }
    }
}
