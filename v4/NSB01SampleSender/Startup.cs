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
    class Startup : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            using (ConsoleColor.Cyan.AsForegroundColor()) 
            {
                Console.WriteLine("Sending MyMessage at startup...");

                var message = new MyMessage()
                {
                    Content = "Hi, there!"
                };

                this.Bus.Send(message);

                Console.WriteLine("MyMessage sent!");
            }
        }

        public void Stop()
        {
            
        }
    }
}
