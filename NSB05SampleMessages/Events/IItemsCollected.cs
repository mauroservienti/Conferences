using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB05SampleMessages.Events
{
    public interface IItemsCollected
    {
        Guid ProcessId { get; set; }
    }
}
