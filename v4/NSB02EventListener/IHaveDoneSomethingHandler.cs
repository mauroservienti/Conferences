using NSB02SampleMessages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace NSB02EventListener
{
    class IHaveDoneSomethingHandler : NServiceBus.IHandleMessages<IHaveDoneSomething>
    {
        public void Handle(IHaveDoneSomething message)
        {
            using (ConsoleColor.Cyan.AsForegroundColor())
            {
                Console.WriteLine("IHaveDoneSomething event received:  {0}", message.JobDone);
            }
        }
    }
}
