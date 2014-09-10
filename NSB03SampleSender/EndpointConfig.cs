
namespace NSB03SampleSender
{
    using NServiceBus;

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
            var embeddedSore = new Raven.Client.Embedded.EmbeddableDocumentStore
            {
                DataDirectory = @"~\..\RavenDB\Data"
            }.Initialize();

            Configure.Instance.DefiningMessagesAs(t => t.Namespace != null && t.Namespace == "NSB03SampleMessages");
            Configure.Instance.RavenPersistenceWithStore(embeddedSore);
        }
    }
}
