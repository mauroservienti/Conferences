
namespace NSB04RavenDB
{
    using NServiceBus;
    using NServiceBus.Persistence;

    /*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<RavenDBPersistence>()
                .DoNotSetupDatabasePermissions()
                .For( Storage.Sagas, Storage.Subscriptions );

            configuration.UsePersistence<InMemoryPersistence>()
                .For( Storage.Timeouts );

        }
    }
}
