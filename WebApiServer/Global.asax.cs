using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Topics.Radical.Bootstrapper;

namespace WebApiServer
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			var formatters = GlobalConfiguration.Configuration.Formatters;
			var jsonFormatter = formatters.JsonFormatter;
			var settings = jsonFormatter.SerializerSettings;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			var directory = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "bin" );

			var bootstrapper = new WindsorBootstrapper( directory, "WebApi*.dll" );
			var container = bootstrapper.Boot();

			SignalRConfig.Config( RouteTable.Routes, container );
			GlobalConfiguration.Configure( cfg => 
			{
				WebApiConfig.Register( cfg, container );
			} );

			FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
			RouteConfig.RegisterRoutes( RouteTable.Routes );
			BundleConfig.RegisterBundles( BundleTable.Bundles );
			JasonConfig.Initialize( directory, "WebApi*", container );

			DependencyResolver.SetResolver( t =>
			{
				if ( container.Kernel.HasComponent( t ) )
				{
					return container.Resolve( t );
				}

				return null;
			},
			t =>
			{
				if ( container.Kernel.HasComponent( t ) )
				{
					return container.ResolveAll( t ).OfType<Object>();
				}

				return new List<Object>();
			} );
		}
	}
}