
namespace MailRecipient
{
    using NServiceBus;
    using NServiceBus.Features;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, INeedInitialization
    {
        public void Init()
        {
            Configure.Features.Disable<SecondLevelRetries>();
            Configure.Instance
                .MsmqSubscriptionStorage()
                .DisableTimeoutManager();
        }
    }
}
