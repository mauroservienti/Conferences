using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Topics.Radical;
using WebApi.Data;

namespace WebApiServer.ComponentModel
{
	public interface IClientSubscriptionManager
	{
		//void RegisterConnectedClient(UserAccount user, String clientId );

		void Subscribe( String clientId, AbstractClientSubscription subscription );
		void UnsubscribeByCorrelationId( string clientId, string correlationId );

		IEnumerable<ClientSubscription> GetClientSubscriptions( string correlationId, IDomainEvent @event );

		IEnumerable<ClientSubscription> GetClientSubscriptions( string correlationId );
	}

	public class ClientSubscription 
	{
		public String ClientId { get; set; }
		public IEnumerable<AbstractClientSubscription> Subscriptions { get; set; }
	}

	public abstract class AbstractClientSubscription
	{
		protected AbstractClientSubscription() { }

		[JsonProperty]
		public Boolean UnsubscribeOnReceive { get; private set; }

		[JsonProperty]
		public String CorrelationId { get; private set; }

		public abstract Boolean Matches( string correlationId, IDomainEvent @event );
	}

	public class EventByPatternSubscription : AbstractClientSubscription
	{
		[JsonConstructor]
		private EventByPatternSubscription() { }

		[JsonProperty]
		public String EventTypePattern { get; private set; }

		public override bool Matches( string correlationId, IDomainEvent @event )
		{
			return this.CorrelationId.Equals( correlationId, StringComparison.OrdinalIgnoreCase )
				&& @event.GetType().FullName.IsLike( this.EventTypePattern );
		}
	}
}
