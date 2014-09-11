using NServiceBus.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB05OrderManager.OrderSaga
{
    public class OrderManagementSagaData : ContainSagaData
    {
        [Unique]
        public Guid ProcessId { get; set; }

        public Boolean CollectionCompleted { get; set; }
    }
}
