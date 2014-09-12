using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSB05WarehouseService.Messages.Commands;
using NSB05WarehouseService.Messages.Events;
using NServiceBus;
using Topics.Radical;

namespace NSB05WarehouseService
{
	class CollectItemsHandler : IHandleMessages<CollectItems>
	{
		public void Handle( CollectItems message )
		{
			using ( ConsoleColor.Magenta.AsForegroundColor() )
			{
				Console.WriteLine( "Collect items request for cart: {0}", message.CartId );

				this.Bus().Publish<IItemsCollected>( e =>
				{
					e.ProcessId = message.ProcessId;
				} );

				Console.WriteLine( "Items collected." );
			}
		}
	}
}
