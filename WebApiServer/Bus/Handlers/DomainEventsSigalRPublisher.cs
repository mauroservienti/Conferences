using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Topics.Radical.Validation;
using WebApi.Data;
using WebApi.Data.People.Events;
using WebApi.Infrastructure;
using WebApiServer.ComponentModel;
using WebApiServer.Runtime;

namespace WebApiServer.Bus.Handlers
{
	public class DomainEventsSigalRPublisher : NServiceBus.IHandleMessages<IDomainEvent>
	{
		readonly IClientSubscriptionManager subscriptionManager;
		readonly IOperationContextManager contextManager;

		public DomainEventsSigalRPublisher( IClientSubscriptionManager subscriptionManager, IOperationContextManager contextManager )
		{
			Ensure.That( subscriptionManager ).Named( () => subscriptionManager ).IsNotNull();
			Ensure.That( contextManager ).Named( () => contextManager ).IsNotNull();

			this.subscriptionManager = subscriptionManager;
			this.contextManager = contextManager;
		}

		public void Handle( IDomainEvent @event )
		{
			var operation = this.contextManager.GetCurrent();
			var correlationId = operation.CorrelationId;

			var clientSubscriptions = this.subscriptionManager.GetClientSubscriptions( correlationId, @event );
			if ( clientSubscriptions.Any() )
			{
				var hubContext = GlobalHost.ConnectionManager.GetHubContext<ClientNotificationsHub>();
				foreach ( var cs in clientSubscriptions )
				{
					var client = hubContext.Clients.Client( cs.ClientId );
					foreach ( var subscription in cs.Subscriptions )
					{
						client.onSubscribedServerEvent( subscription, @event );
					}
				}
			}
		}
	}
}