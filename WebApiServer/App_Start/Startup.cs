using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Owin;

namespace WebApiServer
{
	public class Startup
	{
		// The name *MUST* be Configuration
		public void Configuration( IAppBuilder app )
		{
			app.MapSignalR( new HubConfiguration() { EnableDetailedErrors = true } );
		}
	}
}