using NSB03SampleMessages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace NSB03SampleSender
{
    class Startup : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            using (ConsoleColor.Cyan.AsForegroundColor()) 
            {
                Console.WriteLine("Sending PoisonMessage at startup...");

                var message = new PoisonMessage()
                {
                    Content = "this is expected to fail :-)"
                };

                this.Bus.Send(message);

                Console.WriteLine("PoisonMessage sent!");
            }
        }

        public void Stop()
        {
            
        }
    }
}
