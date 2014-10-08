
namespace NSB08UnitOfWork
{
    using NServiceBus;
    using NServiceBus.UnitOfWork;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
    }

    class CustomInitialization : INeedInitialization
    {
        public void Init()
        {
            Configure.Instance
                .UseInMemoryTimeoutPersister()
                .Configurer.ConfigureComponent<MyUnitOfWork>( DependencyLifecycle.InstancePerUnitOfWork );
        }
    }

    class MyUnitOfWork : IManageUnitsOfWork
    {
        public void Begin() { /* called at the beginning of message handling */ }
        public void End( System.Exception ex = null ) { /* called when message handling is completed */ }
    }

}
