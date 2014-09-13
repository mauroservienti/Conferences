using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSB05ShippingService.Messages.Events;
using Topics.Radical;

namespace NSB05CustomerCare
{
	class OrderShippedHandler : NServiceBus.IHandleMessages<IOrderShipped>
	{
		public void Handle( IOrderShipped message )
		{
			using ( ConsoleColor.Magenta.AsForegroundColor() )
			{
				Console.WriteLine( "We should start a new Saga here to collect feedback." );
			}
		}
	}
}
