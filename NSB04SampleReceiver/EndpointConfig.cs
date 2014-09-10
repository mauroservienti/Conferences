
namespace NSB04SampleReceiver
{
    using NServiceBus;
    using NServiceBus.Features;

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
            Configure.Instance.DefiningMessagesAs(t => t.Namespace != null && t.Namespace == "NSB04SampleMessages");
            Configure.Instance.DisableTimeoutManager();
            Configure.Features.Disable<SecondLevelRetries>();
        }
    }
}
