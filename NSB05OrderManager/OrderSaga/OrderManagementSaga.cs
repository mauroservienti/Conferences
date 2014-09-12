using NServiceBus;
using NServiceBus.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSB05Customer.Messages.Events;
using NSB05OrderManager.Messages.Timeouts;
using NSB05OrderManager.Messages.Events;
using NSB05Common;
using NSB05WarehouseService.Messages.Commands;
using NSB05WarehouseService.Messages.Events;

namespace NSB05OrderManager.OrderSaga
{
    public class OrderManagementSaga
        : Saga<OrderManagementSagaData>,
        IAmStartedByMessages<IShoppingCartCheckedout>,
		//IHandleMessages<IItemsCollected>,
        IHandleTimeouts<ItemCollectionTimeout>
    {
		//public override void ConfigureHowToFindSaga()
		//{
		//	this.ConfigureMapping<IItemsCollected>(d => d.ProcessId, m => m.ProcessId);
		//}

        public void Handle(IShoppingCartCheckedout message)
        {
            this.Data.ProcessId = Guid.NewGuid().ToString();
			this.Data.CartId = message.CartId;

			this.Bus.Send( new CollectItems()
			{
				ProcessId = this.Data.ProcessId,
				CartId = this.Data.CartId
			} );

            this.RequestTimeout(TimeSpan.FromSeconds(10), new ItemCollectionTimeout()
            {
                ProcessId = this.Data.ProcessId
            });
        }

		public void Handle( IItemsCollected message )
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
