
namespace MailSender
{
    using NServiceBus;
    using NServiceBus.Features;
    using System;
    using System.Threading;
    using Topics.Radical;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, INeedInitialization
    {
        public void Init()
        {
            Configure.Features.Disable<SecondLevelRetries>();
            Configure.Instance
                .MsmqSubscriptionStorage()
                .DisableTimeoutManager();
        }
    }

    public class Runner : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            while ( true )
            {
                var msg = new Messages.DeliverMailMessage()
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Subject = "Hi, there!"
                };

                this.Bus.Send( msg );

                using ( ConsoleColor.Cyan.AsForegroundColor() )
                {
                    Console.WriteLine( "Sent a message from with ID '{0}'", msg.MessageId );
                }

                Thread.Sleep( 500 );
            }
        }

        public void Stop()
        {

        }
    }

}
