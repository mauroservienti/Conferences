using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;
using NSB05Customer.Messages.Events;

namespace NSB05Customer
{
    class Startup : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            using (ConsoleColor.Cyan.AsForegroundColor()) 
            {
				Console.WriteLine( "Publishing IShoppingCartCheckedout..." );

				this.Bus.Publish<IShoppingCartCheckedout>( e => 
				{
					e.CartId = Guid.NewGuid().ToString();
				} );

				Console.WriteLine( "IShoppingCartCheckedout published." );
            }
        }

        public void Stop()
        {
            
        }
    }
}
