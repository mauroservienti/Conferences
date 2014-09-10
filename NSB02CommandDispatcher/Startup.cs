using NSB02SampleMessages;
using NSB02SampleMessages.Commands;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace NSB02CommandDispatcher
{
    class Startup : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            using (ConsoleColor.Cyan.AsForegroundColor())
            {
                Console.WriteLine("Sending DoSomethingForMe at startup...");

                var command = new DoSomethingForMe()
                {
                    Content = "Hi, there!"
                };

                this.Bus.Send(command);

                Console.WriteLine("DoSomethingForMe sent!");
            }
        }

        public void Stop()
        {

        }
    }
}
