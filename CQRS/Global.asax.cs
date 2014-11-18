using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using Topics.Radical.Bootstrapper;
using Topics.Radical.Bootstrapper.Windsor.AspNet.Infrastructure;

namespace CQRS
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			var path = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "bin" );
			var bootstrapper = new WindsorBootstrapper( path, "CQ*" );
			var container = bootstrapper.Boot();

			GlobalConfiguration.Configure( cfg =>
			{
				WebApiConfig.Register( cfg, container );
			} );
			JasonConfig.Initialize( path, "CQ*", container );
		}
	}
}
