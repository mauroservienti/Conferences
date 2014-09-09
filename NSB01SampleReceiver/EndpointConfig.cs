
namespace NSB01SampleReceiver
{
    using NServiceBus;
    using Raven.Client.Embedded;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
    }

    public class CustomInitialization : IWantCustomInitialization
    {
        public void Init()
        {
            var embeddedSore = new EmbeddableDocumentStore
            {
                DataDirectory = @"~\RavenDB\Data"
            }.Initialize();

            Configure.Instance.DefiningMessagesAs(t => t.Namespace != null && t.Namespace == "NSB01SampleMessages");
            Configure.Instance.RavenPersistenceWithStore(embeddedSore);
        }
    }
}
