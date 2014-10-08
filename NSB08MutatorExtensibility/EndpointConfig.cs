
namespace NSB08MutatorExtensibility
{
    using NServiceBus;
    using NServiceBus.MessageMutator;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server { }

    class CustomInitialization : INeedInitialization
    {
        public void Init()
        {
            Configure.Instance
                .UseInMemoryTimeoutPersister()
                .Configurer.ConfigureComponent<MyMultiPurposeMutator>( DependencyLifecycle.SingleInstance );
        }
    }

    class MyMultiPurposeMutator :
        IMutateOutgoingTransportMessages,
        IMutateIncomingTransportMessages,
        IMutateIncomingMessages,
        IMutateOutgoingMessages
    {
        public void MutateOutgoing( object[] messages, TransportMessage transportMessage ) { }
        public void MutateIncoming( TransportMessage transportMessage ) { }
        public object MutateIncoming( object message ) { return message; }
        public object MutateOutgoing( object message ) { return message; }
    }

}
