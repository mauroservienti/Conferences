using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using NServiceBus;
using Topics.Radical.Validation;
using Topics.Radical.Reflection;
using Topics.Radical;
using WebApi.Data;
using Raven.Client;
using Topics.Radical.Bootstrapper;
using NServiceBus.Serialization;

namespace WebApiServer.Configurations
{
	class ServiceBusConfiguration : IConfigurator
	{
		public void Configure( IServiceProvider serviceProvider )
		{
			var container = ( ( IServiceProviderWrapper )serviceProvider ).Unwrap<IWindsorContainer>();

			NServiceBus.Configure.Serialization.AdvancedJson();
			var docStore = container.Resolve<IDocumentStore>();

			var ub = NServiceBus.Configure.With()
				.CastleWindsorBuilder( container )
				.DefiningMessagesAs( t =>
				{
					if ( !t.IsAbstract && t.Namespace != null && !t.IsNested )
					{
						return t.Is<IMessage>() || ( t.Namespace != null && t.Namespace.IsLike( "WebApi*.Messages" ) && t.Name.EndsWith( "Message" ) );
					}

					return false;
				} )
				.DefiningCommandsAs( t =>
				{
					if ( !t.IsAbstract && t.Namespace != null && !t.IsNested )
					{
						return t.Is<ICommand>() || ( t.Namespace != null && t.Namespace.IsLike( "WebApi*.Commands" ) && t.Name.EndsWith( "Command" ) );
					}

					return false;
				} )
				.DefiningEventsAs( t =>
				{
					if ( t.Is<IDomainEvent>() ) 
					{
						return true;
					}

					if ( !t.IsAbstract && t.Namespace != null && !t.IsNested )
					{
						return t.Is<IEvent>()
							|| t.Is<IDomainEvent>()
							|| ( t.Namespace != null && t.Namespace.IsLike( "WebApi*.Events" ) && t.Name.EndsWith( "Event" ) );
					}
					return false;
				} )
				.Log4Net()
				.UseTransport<Msmq>()
				.RavenPersistenceWithStore( docStore )
				.RavenSubscriptionStorage()
				.UnicastBus()
				.WithContextHandling()
				.LoadMessageHandlers()
				.CreateBus()
				.Start( () => NServiceBus.Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install() );
		}
	}
}
