using System;
using System.Linq;
using System.Collections.Generic;
using Raven.Imports.Newtonsoft.Json;

namespace WebApi.Data
{
	public abstract class Aggregate : IAggregate, IEquatable<IAggregate>
	{
		private String _id;

		public String Id
		{
			get { return _id; }
			private set 
			{
				if ( this._id != value )
				{
					this._id = value;
					this.OnIdUpdated();
				}
			}
		}

		private void OnIdUpdated()
		{
			if ( this.IsChanged ) 
			{
				foreach ( var @event in this.uncommittedEvents ) 
				{
					( ( DomainEvent )@event ).AggregateId = this.Id;
				}
			}
		}

		public int Version { get; private set; }

		[JsonIgnore]
		public Boolean IsChanged { get { return this.uncommittedEvents.Any(); } }

		[JsonIgnore]
		List<IDomainEvent> uncommittedEvents = new List<IDomainEvent>();

		IEnumerable<IDomainEvent> IAggregate.GetUncommittedEvents()
		{
			return this.uncommittedEvents.ToArray();
		}

		void IAggregate.ClearUncommittedEvents()
		{
			this.uncommittedEvents.Clear();
		}

		protected void RaiseEvent( IDomainEvent @event )
		{
			this.uncommittedEvents.Add( @event );
			this.Version++;
		}

		public override Int32 GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		public override Boolean Equals( object obj )
		{
			return this.Equals( obj as IAggregate );
		}

		public virtual Boolean Equals( IAggregate other )
		{
			return other != null && other.Id == this.Id;
		}
	}
}
