using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSB05ShippingService.Messages.Commands;
using NSB05ShippingService.Messages.Events;
using NServiceBus;
using Topics.Radical;

namespace NSB05ShippingService
{
	class ShipOrderHandler : NServiceBus.IHandleMessages<ShipOrder>
	{
		public void Handle( ShipOrder message )
		{
			using ( ConsoleColor.Cyan.AsForegroundColor() )
			{
				Console.WriteLine( "Shipping order {0}...", message.OrderId );

				this.Bus()
					.Publish<IOrderShipped>( e =>
					{
						e.OrderId = message.OrderId;
					} );

				Console.WriteLine( "Order shipped." );
			}
		}
	}
}
