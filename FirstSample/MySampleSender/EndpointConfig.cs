
namespace MySampleSender
{
    using NServiceBus;

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
