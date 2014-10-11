using NSB02SampleMessages.Commands;
using NSB02SampleMessages.Events;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace NSB02CommandExecutor
{
    class DoSomethingForMeHandler : IHandleMessages<DoSomethingForMe>
    {
        public void Handle(DoSomethingForMe message)
        {
            using (ConsoleColor.Cyan.AsForegroundColor())
            {
                Console.WriteLine("Received DoSomethingForMe command: {0}", message.Content);

                this.Bus().Publish<IHaveDoneSomething>(e => 
                {
                    e.JobDone = message.Content;
                });

                Console.WriteLine("Job done.");
            }
        }
    }
}
