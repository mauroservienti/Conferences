using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using Topics.Radical;

namespace MySampleSender
{
	class StartupTask : IWantToRunWhenBusStartsAndStops
	{
		public IBus Bus { get; set; }

		public void Start()
		{
			while ( true )
			{
				var msg = new SampleMessages.AMessage()
				{
					SomethingToSay = "Hi, there: " + Guid.NewGuid().ToString()
				};

				this.Bus.Send( msg );

				using ( ConsoleColor.Cyan.AsForegroundColor() )
				{
					Console.WriteLine( "Message sent: {0}", msg.SomethingToSay );
				}

				Thread.Sleep( 500 );
			}
		}

		public void Stop()
		{

		}
	}
}
