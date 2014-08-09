using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public class DeliverMailMessage : ICommand
    {
        public String MessageId { get; set; }
        public String Subject { get; set; }

        public String Body { get; set; }
    }
}
