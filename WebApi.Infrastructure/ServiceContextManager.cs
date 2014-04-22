using System;
using System.Security.Principal;
using System.Threading;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Config;
using NServiceBus.UnitOfWork;
using NServiceBus.Transports;
using WebApi.Infrastructure;

namespace WebApi.Infrastructure
{
	public class ServiceContextManager :
		IManageUnitsOfWork,
		IMutateOutgoingTransportMessages
	{
		const String CORRELATION_ID_HEADER = "operationContext/correlationId";

		static readonly ILog logger = LogManager.GetLogger( typeof( ServiceContextManager ) );

		readonly IBus bus;
		readonly IOperationContextManager contextManager;
		readonly ISendMessages messageSender;

		public ServiceContextManager( IBus bus, ISendMessages messageSender, IOperationContextManager contextManager )
		{
			this.bus = bus;
			this.messageSender = messageSender;
			this.contextManager = contextManager;
		}

		void IMutateOutgoingTransportMessages.MutateOutgoing( object[] messages, TransportMessage transportMessage )
		{
			if ( transportMessage.MessageIntent == MessageIntentEnum.Subscribe )
			{
				return;
			}

			if ( transportMessage.MessageIntent == MessageIntentEnum.Unsubscribe )
			{
				return;
			}

			if ( ConfigureServiceContextManager.HandleOperationContext )
			{
				var correlationId = this.contextManager.GetCurrent().CorrelationId;

				transportMessage.Headers[ CORRELATION_ID_HEADER ] = correlationId ?? "";
			}
		}

		public void Begin()
		{
			if ( ConfigureServiceContextManager.HandleOperationContext )
			{
				var messageContext = this.bus.CurrentMessageContext;
				if ( messageContext.Headers.ContainsKey( CORRELATION_ID_HEADER ) )
				{
					var correlationId = messageContext.Headers[ CORRELATION_ID_HEADER ];

					this.contextManager.GetCurrent().ForOperation( correlationId );
				}
			}
		}

		public void End( Exception ex = null )
		{

		}
	}
}

namespace NServiceBus
{
	public static class ConfigureServiceContextManager
	{
		static ConfigureServiceContextManager()
		{
			HandleOperationContext = false;
		}

		public static ConfigUnicastBus WithContextHandling( this ConfigUnicastBus config, Boolean handleOperationContext = true )
		{
			if ( handleOperationContext )
			{
				HandleOperationContext = true;
			}

			Configure.Instance
				.Configurer
				.ConfigureComponent<ServiceContextManager>( DependencyLifecycle.SingleInstance );


			return config;
		}

		public static bool HandleOperationContext { get; private set; }
	}
}