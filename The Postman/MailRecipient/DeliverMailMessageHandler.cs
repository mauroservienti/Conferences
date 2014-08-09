using Messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace MailRecipient
{
    class DeliverMailMessageHandler : IHandleMessages<DeliverMailMessage>
    {
        public IBus Bus { get; set; }

        public void Handle( DeliverMailMessage message )
        {
            var me = this.GetType().Namespace.Split('.').Last();
            var source = this.Bus.GetMessageHeader( message, NServiceBus.Headers.OriginatingEndpoint );
            var id = message.MessageId;

            using ( ConsoleColor.Cyan.AsForegroundColor() )
            {
                Console.WriteLine("I am {0}, received a message from '{1}' with ID '{2}'", me, source, id );
            }

            this.Bus.Publish<IMailMessageDelivered>( e => 
            {
                e.MessageId = message.MessageId;
            } );

            using ( ConsoleColor.Cyan.AsForegroundColor() )
            {
                Console.WriteLine( "message forwarded..." );
            }
        }
    }
}
