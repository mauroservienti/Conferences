using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using SampleMessages;
using Topics.Radical;

namespace MySampleReceiver
{
	class AMessageHandler : IHandleMessages<AMessage>
	{
		public void Handle( AMessage message )
		{
			using ( ConsoleColor.Cyan.AsForegroundColor() )
			{
				Console.WriteLine( "Message received: {0}", message.SomethingToSay );
			}
		}
	}
}
