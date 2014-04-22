using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Topics.Radical.Validation;
using WebApiServer.ComponentModel;

namespace WebApiServer.Runtime
{
	public class ClientNotificationsHub : Hub
	{
		readonly IClientSubscriptionManager subscriptionManager;

		public ClientNotificationsHub( IClientSubscriptionManager subscriptionManager )
		{
			Ensure.That( subscriptionManager ).Named( () => subscriptionManager ).IsNotNull();

			this.subscriptionManager = subscriptionManager;
		}

		public override System.Threading.Tasks.Task OnConnected()
		{
			var ctx = this.Context;

			return base.OnConnected();
		}

		public override System.Threading.Tasks.Task OnDisconnected()
		{
			var ctx = this.Context;

			return base.OnDisconnected();
		}

		public override System.Threading.Tasks.Task OnReconnected()
		{
			var ctx = this.Context;

			return base.OnReconnected();
		}

		public void NotifyOnEventByPattern( EventByPatternSubscription subscription )
		{
			var clientId = this.Context.ConnectionId;

			this.subscriptionManager.Subscribe( clientId, subscription );
		}

		public void UnsubscribeByCorrelationId( String correlationId )
		{
			var clientId = this.Context.ConnectionId;

			this.subscriptionManager.UnsubscribeByCorrelationId( clientId, correlationId );
		}
	}
}