using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Topshelf;
using WebApi.Data;
using Topics.Radical;
using Topics.Radical.Reflection;
using Raven.Client;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Topics.Radical.Bootstrapper;
using NServiceBus.Serialization;

namespace WebApi.Lurker
{
	class Program
	{
		static void Main( string[] args )
		{
			var bootstrapper = new WindsorBootstrapper( AppDomain.CurrentDomain.BaseDirectory, "WebApi*.dll" );
			bootstrapper.AddCatalog( new AssemblyCatalog( Assembly.GetExecutingAssembly() ) );

			var delayedStartup = bootstrapper.DelayedBoot();
			var configuration = new ServiceBusHostingConfiguration( delayedStartup.Container );

			var host = HostFactory.New( x =>
			{
				x.Service<Hosting>( () => new Hosting( configuration, cfg => { }, () => delayedStartup.Startup() ) );
				x.StartAutomatically();
				x.RunAsNetworkService();
			} );

			host.Run();
		}
	}

	public class ServiceBusHostingConfiguration
	{
		private Castle.Windsor.IWindsorContainer container;

		public ServiceBusHostingConfiguration( Castle.Windsor.IWindsorContainer container )
		{
			this.container = container;
		}

		public Boolean DisableContextHandling { get; set; }
		public Boolean DisableTransactions { get; set; }

		internal void Intialize()
		{
			if ( this.DisableTransactions )
			{
				NServiceBus.Configure.Transactions
					.Disable()
					.Advanced( s =>
					{
						s.DisableDistributedTransactions();
						s.DoNotWrapHandlersExecutionInATransactionScope();
					} );
			}

			Configure.Serialization.AdvancedJson();

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
				.Sagas()
				.PurgeOnStartup( false )
				.MessageForwardingInCaseOfFault()
				.UnicastBus()
				.LoadMessageHandlers();

			if ( !this.DisableContextHandling )
			{
				ub.WithContextHandling();
			}

			var bus = ub.CreateBus()
				.Start( () => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install() );
		}
	}

	class Hosting : ServiceControl
	{
		readonly ServiceBusHostingConfiguration configuration;
		readonly Action<ServiceBusHostingConfiguration> beforeInit;
		readonly Action startup;

		public Hosting( ServiceBusHostingConfiguration configuration, Action<ServiceBusHostingConfiguration> beforeInit, Action startup )
		{
			this.configuration = configuration;
			this.beforeInit = beforeInit;
			this.startup = startup;
		}

		public bool Start( HostControl hostControl )
		{
			this.beforeInit( this.configuration );
			this.configuration.Intialize();

			this.startup();

			return true;
		}

		public bool Stop( HostControl hostControl )
		{
			return true;
		}
	}
}
