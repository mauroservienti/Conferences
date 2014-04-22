using System;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using WebApi.Data.Commits;

namespace WebApi.Data.Indexes
{
	public class Events_Stream : AbstractIndexCreationTask<Commit, Events_Stream.Event>
	{
		public class Event 
		{
			public String CommitId { get; private set; }
			public DateTimeOffset CreatedOn { get; private set; }
			public String AggregateIdentifier { get; private set; }
			public String CorrelationId { get; private set; }
			public String EventType { get; private set; }
			public IDomainEvent EventData { get; private set; }
		}

		public Events_Stream()
		{
			this.Map = docs => from doc in docs
							   from evt in doc.Events
							   select new
							   {
								   CommitId = doc.Id,
								   doc.CreatedOn,
								   doc.AggregateIdentifier,
								   doc.CorrelationId,
								   EventType = AsDocument( evt ).Value<string>( "$type" ),
								   EventData = evt,
							   };

			this.StoreAllFields( FieldStorage.Yes );
		}
	}
}
