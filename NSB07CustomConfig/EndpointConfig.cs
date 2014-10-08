
namespace NSB07CustomConfig
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
        public void Init()
        {
            //IWantCustomInitialization this should be implemented on the EndpointConfig class.
            //No DI here
            //Need to call Configure.With()
        }
    }

    class CustomInitialization : INeedInitialization
    {
        public void Init()
        {
            //INeedInitialization cannot be implemented on the EndpointConfig class
            //Called after Configure.With() is completed and a container has been set.
            Configure.Instance.UseInMemoryTimeoutPersister();
        }
    }

}
