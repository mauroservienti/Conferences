using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Data;
using WebApiServer.ComponentModel;

namespace WebApiServer.Services
{
	class ClientSubscriptionManager : IClientSubscriptionManager
	{
		//BUG: should we lock?
		Dictionary<String, List<AbstractClientSubscription>> clientSubscriptions = new Dictionary<string, List<AbstractClientSubscription>>();

		public IEnumerable<ClientSubscription> GetClientSubscriptions( string correlationId )
		{
			var subscriptions = this.clientSubscriptions
				.Where( kvp => kvp.Value.Any( s => String.Equals( s.CorrelationId, correlationId, StringComparison.OrdinalIgnoreCase ) ) )
				.Select( kvp => new ClientSubscription()
				{
					ClientId = kvp.Key,
					Subscriptions = kvp.Value.Where( s => String.Equals( s.CorrelationId, correlationId, StringComparison.OrdinalIgnoreCase ) ).ToArray()
				} )
				.ToArray();

			return subscriptions;
		}

		public IEnumerable<ClientSubscription> GetClientSubscriptions( string correlationId, IDomainEvent @event )
		{
			var subscriptions = this.clientSubscriptions
				.Where( kvp => kvp.Value.Any( s => s.Matches( correlationId, @event ) ) )
				.Select( kvp => new ClientSubscription()
				{
					ClientId = kvp.Key,
					Subscriptions = kvp.Value.Where( s => s.Matches( correlationId, @event ) ).ToArray()
				} )
				.ToArray();

			return subscriptions;
		}

		public void UnsubscribeByCorrelationId( String clientId, String correlationId )
		{
			List<AbstractClientSubscription> subscriptions;
			if ( this.clientSubscriptions.TryGetValue( clientId, out subscriptions ) )
			{
				subscriptions.RemoveAll( s => s.CorrelationId.Equals( correlationId, StringComparison.OrdinalIgnoreCase ) );
			}
		}

		public void Subscribe( String clientId, AbstractClientSubscription subscription )
		{
			List<AbstractClientSubscription> subscriptions;
			if ( !this.clientSubscriptions.TryGetValue( clientId, out subscriptions ) )
			{
				subscriptions = new List<AbstractClientSubscription>();
				this.clientSubscriptions.Add( clientId, subscriptions );
			}

			subscriptions.Add( subscription );
		}
	}
}