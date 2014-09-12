using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSB05OrderManager.Messages.Events;
using Topics.Radical;

namespace NSB05CustomerCare
{
	class OrderProcessingDelayedHandler : NServiceBus.IHandleMessages<IOrderProcessingDelayed>
	{
		public void Handle( IOrderProcessingDelayed message )
		{
			using ( ConsoleColor.Red.AsForegroundColor() ) 
			{
				Console.WriteLine("We should send a gift to the customer...the order has been dalyed.");
			}
		}
	}
}
