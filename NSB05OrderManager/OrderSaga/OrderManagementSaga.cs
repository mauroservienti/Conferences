using NSB05SampleMessages;
using NSB05SampleMessages.Commands;
using NSB05SampleMessages.Events;
using NSB05SampleMessages.Timeouts;
using NServiceBus;
using NServiceBus.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB05OrderManager.OrderSaga
{
    public class OrderManagementSaga
        : Saga<OrderManagementSagaData>,
        IAmStartedByMessages<IShoppingCartCheckedout>,
        IHandleMessages<IItemsCollected>,
        IHandleTimeouts<ItemCollectionTimeout>
    {
        public override void ConfigureHowToFindSaga()
        {
            this.ConfigureMapping<IItemsCollected>(d => d.ProcessId, m => m.ProcessId);
        }

        public void Handle(IShoppingCartCheckedout message)
        {
            this.Data.ProcessId = Guid.NewGuid();

            this.Bus.Send(new CollectItemsForShipping()
            {
                ProcessId = this.Data.ProcessId
            });

            this.RequestTimeout(TimeSpan.FromSeconds(10), new ItemCollectionTimeout()
            {
                ProcessId = this.Data.ProcessId
            });
        }

        public void Handle(IItemsCollected message)
        {
            this.Data.CollectionCompleted = true;
        }

        public void Timeout(ItemCollectionTimeout state)
        {
            if (!this.Data.CollectionCompleted) 
            {
                this.Bus.Publish<IOrderProcessingDelayed>(e => 
                {
                    e.ProcessId = this.Data.ProcessId;
                    e.Reason = ProcessingDelayReason.SlowItemCollection;
                });
            }
        }
    }
}
