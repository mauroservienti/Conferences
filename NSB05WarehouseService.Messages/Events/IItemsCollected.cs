using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB05WarehouseService.Messages.Events
{
    public interface IItemsCollected
    {
        String ProcessId { get; set; }
    }
}
