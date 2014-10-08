using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace NSB06SelfHost
{
    class Program
    {
        static void Main( string[] args )
        {
            var embeddedSore = new Raven.Client.Embedded.EmbeddableDocumentStore
            {
                DataDirectory = @"~\..\RavenDB\Data"
            }.Initialize();

            NServiceBus.Configure.With()
                .DefaultBuilder()
                .Log4Net()
                .DefiningCommandsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) )
                .DefiningEventsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Events" ) )
                .RavenPersistenceWithStore( embeddedSore )
                .UnicastBus()
                .LoadMessageHandlers()
                .CreateBus()
                .Start();

            Console.Read();
        }
    }
}
