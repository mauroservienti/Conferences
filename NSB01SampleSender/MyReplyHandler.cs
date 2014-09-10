using NSB01SampleMessages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace NSB01SampleSender
{
    class MyReplyHandler : IHandleMessages<MyReply>
    {
        public IBus Bus { get; set; }

        public void Handle(MyReply message)
        {
            using (ConsoleColor.Cyan.AsForegroundColor())
            {
                Console.WriteLine("Received MyReply from:  {0}", this.Bus.CurrentMessageContext.ReplyToAddress);
            }
        }
    }
}
