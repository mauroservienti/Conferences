using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jason.Handlers.Commands;
using NServiceBus;

namespace WebApiServer.Infrastructure
{
	public class JasonToBusRepublishHandler : ICommandHandler
	{
		readonly IBus serviceBus;

		public JasonToBusRepublishHandler( IBus serviceBus )
		{
			this.serviceBus = serviceBus;
		}

		public object Execute( object command )
		{
			this.serviceBus.Send( command );

			return Jason.Defaults.Response.Ok;
		}
	}
}
