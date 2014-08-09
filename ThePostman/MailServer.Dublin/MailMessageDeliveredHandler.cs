using Messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace MailServer.Dublin
{
    class MailMessageDeliveredHandler : IHandleMessages<IMailMessageDelivered>
    {
        public IBus Bus { get; set; }

        public void Handle( IMailMessageDelivered message )
        {
            var me = this.GetType().Namespace.Split( '.' ).Last();
            var source = this.Bus.GetMessageHeader( message, NServiceBus.Headers.OriginatingEndpoint );
            var id = message.MessageId;

            using ( ConsoleColor.Yellow.AsForegroundColor() )
            {
                Console.WriteLine( "I am {0}, received a delivery receipt from '{1}' with ID '{2}'", me, source, id );
            }

            this.Bus.Publish( message );

            using ( ConsoleColor.Yellow.AsForegroundColor() )
            {
                Console.WriteLine( "Receipt published." );
            }
        }
    }
}
