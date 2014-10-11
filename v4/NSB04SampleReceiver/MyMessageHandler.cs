using NSB04SampleMessages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace NSB04SampleReceiver
{
    class MyMessageHandler : IHandleMessages<MyMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(MyMessage message)
        {
            using (ConsoleColor.Cyan.AsForegroundColor())
            {
                Console.WriteLine("Handling MyMessage: {0}", message.Content);
            }
        }
    }
}
