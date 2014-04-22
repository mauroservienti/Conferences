using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Validation;
using Topics.Radical.Linq;

namespace WebApi.Data.Commits
{
	public class Commit
	{
		public class Factory
		{
			public Commit CreateFor( Guid transactionIdentifier, string correlationId, IAggregate aggregate, String userAccount )
			{
				var commit = new Commit( transactionIdentifier, correlationId, aggregate.Id, aggregate.GetUncommittedEvents(), userAccount );

				return commit;
			}
		}

		[Raven.Imports.Newtonsoft.Json.JsonConstructor]
		private Commit()
		{

		}

		private Commit( Guid transactionIdentifier, String correlationId, String aggregateId, IEnumerable<IDomainEvent> uncommittedEvents, String userAccount )
		{
			this.CreatedOn = DateTimeOffset.Now;
			this.TransactionIdentifier = transactionIdentifier.ToString();
			this.CorrelationId = correlationId;
			this.AggregateIdentifier = aggregateId;
			this.Events = uncommittedEvents;
			this.UserAccount = userAccount;
		}

		private String _id;

		public String Id
		{
			get { return this._id; }
			private set
			{
				if ( this.Id != value )
				{
					this._id = value;
					if ( this.Events != null && this.Events.Any( e => e.Id == null ) )
					{
						this.Events.ForEach( 1, ( idx, evt ) =>
						{
							( ( DomainEvent )evt ).Id = this.Id + "/events/" + idx;

							return ++idx;
						} );
					}
				}
			}
		}
		public DateTimeOffset CreatedOn { get; private set; }
		public String TransactionIdentifier { get; private set; }
		public String CorrelationId { get; private set; }
		public String AggregateIdentifier { get; private set; }
		public IEnumerable<IDomainEvent> Events { get; private set; }

		public Boolean IsDispatched { get; private set; }

		public void MarkAsDispatched()
		{
			this.IsDispatched = true;
		}

		public String UserAccount { get; private set; }
	}
}
