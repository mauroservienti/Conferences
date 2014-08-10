
namespace MySampleReceiver
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, INeedInitialization
	{
		public void Init()
		{
			Configure.Features.Disable<NServiceBus.Features.SecondLevelRetries>();
			Configure.Instance
				.MsmqSubscriptionStorage()
				.DisableTimeoutManager();
		}
	}
}
