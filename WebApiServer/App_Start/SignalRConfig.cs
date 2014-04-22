using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Json;
using Newtonsoft.Json;
using Topics.Radical.Reflection;
using WebApi.Data;
using WebApiServer.ComponentModel;
using WebApiServer.Infrastructure;

namespace WebApiServer
{
	public static class SignalRConfig
	{
		public static void Config( RouteCollection routes, IWindsorContainer container )
		{
			GlobalHost.DependencyResolver = new SignalRDependencyResolver( container );

			var serializer = new JsonSerializer()
			{
				ContractResolver = new FilteredCamelCasePropertyNamesContractResolver
				{
					ShouldConvertToCamelCase = t => t.Is<AbstractClientSubscription>()
												 || t.Is<IDomainEvent>(),
					//TypesToInclude = { typeof( ClientSubscription ) }
				}
			};

			GlobalHost.DependencyResolver.Register( typeof( JsonSerializer ), () => serializer );
			GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds( 5 );
		}
	}

	class SignalRDependencyResolver : DefaultDependencyResolver
	{
		IWindsorContainer container;

		public SignalRDependencyResolver( IWindsorContainer container )
		{
			this.container = container;
		}

		public override void Register( Type serviceType, Func<object> activator )
		{
			if ( this.container == null )
			{
				base.Register( serviceType, activator );
			}
			else
			{
				this.container.Register( Component.For( serviceType ).UsingFactoryMethod( () =>
				{
					return activator();
				} ) );
			}
		}

		public override object GetService( Type serviceType )
		{
			if ( this.container.Kernel.HasComponent( serviceType ) )
			{
				return this.container.Resolve( serviceType );
			}
			return base.GetService( serviceType );
		}

		public override IEnumerable<object> GetServices( Type serviceType )
		{
			if ( this.container.Kernel.HasComponent( serviceType ) )
			{
				return this.container.ResolveAll( serviceType ).Cast<Object>();
			}

			return base.GetServices( serviceType );
		}
	}
}