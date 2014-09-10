using NSB03SampleMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace NSB03SampleReceiver
{
    class PoisonMessageHandler : NServiceBus.IHandleMessages<PoisonMessage>
    {
        public void Handle(PoisonMessage message)
        {
            Console.WriteLine("Handling PoisonMessage...");

            throw new NotImplementedException("ahahahahahahaha :-D");
        }
    }
}
