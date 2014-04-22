using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Routing;
using WebApiServer.Infrastructure;

namespace WebApiServer
{
	public static class WebApiConfig
	{
		public static void Register( HttpConfiguration config, Castle.Windsor.IWindsorContainer container )
		{
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				"DefaultApiWithId",
				"api/{controller}/{id}",
				new { id = RouteParameter.Optional },
				new { id = @"\d+" } );

			config.Routes.MapHttpRoute(
				"CommitsApiWithId",
				"api/Commits/{commitId}/Events",
				new { controller = "Commits", action = "Events" },
				new { commitId = @"\d+", } );

			config.Routes.MapHttpRoute(
				"CommitsEventsApiWithId",
				"api/Commits/{commitId}/Events/{eventId}",
				new 
				{
					eventId = RouteParameter.Optional, 
					controller = "Commits" 
				},
				new 
				{ 
					commitId = @"\d+", 
					eventId = @"\d+" 
				} );

			config.Routes.MapHttpRoute(
				"DefaultApiWithAction",
				"api/{controller}/{action}" );

			config.Routes.MapHttpRoute(
				"DefaultApiGet",
				"api/{controller}",
				new { action = "Get", controller = "Root" },
				new { httpMethod = new HttpMethodConstraint( HttpMethod.Get ) } );

			config.Routes.MapHttpRoute(
				"DefaultApiPost",
				"api/{controller}",
				new { action = "Post" },
				new { httpMethod = new HttpMethodConstraint( HttpMethod.Post ) } );

			// Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
			// To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
			// For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
			//config.EnableQuerySupport();

			// To disable tracing in your application, please comment out or remove the following line of code
			// For more information, refer to: http://www.asp.net/web-api
			//config.EnableSystemDiagnosticsTracing();

			var allFilters = container.ResolveAll<IFilter>();
			if ( allFilters.Any() )
			{
				foreach ( var filter in allFilters )
				{
					config.Filters.Add( filter );
				}
			}

			GlobalConfiguration.Configuration.DependencyResolver = new DelegateDependencyResolver()
			{
				OnGetService = t =>
				{
					if ( container.Kernel.HasComponent( t ) )
					{
						return container.Resolve( t );
					}

					return null;
				},
				OnGetServices = t =>
				{
					if ( container.Kernel.HasComponent( t ) )
					{
						return container.ResolveAll( t ).OfType<Object>();
					}

					return new List<Object>();
				}
			};
		}
	}
}
