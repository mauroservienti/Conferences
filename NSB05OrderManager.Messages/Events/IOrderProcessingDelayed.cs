using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSB05Common;

namespace NSB05OrderManager.Messages.Events
{
    public interface IOrderProcessingDelayed
    {

        String ProcessId { get; set; }

        ProcessingDelayReason Reason { get; set; }
    }
}
