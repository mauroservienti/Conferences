using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Validation;

namespace WebApi.Data
{
	public abstract class DomainEvent : IDomainEvent
	{
		protected DomainEvent()
		{
		}

		protected DomainEvent( String aggregateId )
		{
			//Ensure.That( aggregateId ).Named( () => aggregateId ).IsNotNullNorEmpty();

			this.OccurredAt = DateTimeOffset.Now;
			this.AggregateId = aggregateId;
		}

		public string Id
		{
			get;
			set;
		}

		public string AggregateId
		{
			get;
			set;
		}

		public DateTimeOffset OccurredAt
		{
			get;
			private set;
		}
	}
}
