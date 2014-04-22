using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using WebApi.Data.Commits;

namespace WebApi.Data.Indexes
{
	public class History_Stream : AbstractMultiMapIndexCreationTask<History_Stream.Moment>
	{
		public enum MomentType
		{
			Event,
			Command,
			Response,
			Exception
		}

		public class Moment
		{
			public String Id { get; private set; }
			public DateTimeOffset CreatedOn { get; private set; }
			public String AggregateIdentifier { get; private set; }
			public String CorrelationId { get; private set; }
			public String UserAccount { get; private set; }
			public String Type { get; private set; }
			public Object Data { get; private set; }
			public MomentType MomentType { get; private set; }
		}

		public History_Stream()
		{
			this.AddMap<Commit>( docs => from doc in docs
										 from evt in doc.Events
										 select new
										 {
											 evt.Id,
											 doc.CreatedOn,
											 doc.AggregateIdentifier,
											 doc.CorrelationId,
											 doc.UserAccount,
											 Type = AsDocument( evt ).Value<string>( "$type" ),
											 Data = evt,
											 MomentType = MomentType.Event,
										 } );

			this.AddMap<Requests.CommandRequest>( docs => from doc in docs
														  let command = doc.Command
														  select new
														  {
															  doc.Id,
															  doc.CreatedOn,
															  AggregateIdentifier = "",
															  doc.CorrelationId,
															  doc.UserAccount,
															  Type = AsDocument( command ).Value<string>( "$type" ),
															  Data = command,
															  MomentType = MomentType.Command,
														  } );

			this.AddMap<Requests.CommandResponse>( docs => from doc in docs
														   let response = doc.Response
														   select new
														   {
															   doc.Id,
															   doc.CreatedOn,
															   AggregateIdentifier = "",
															   doc.CorrelationId,
															   doc.UserAccount,
															   Type = AsDocument( response ).Value<string>( "$type" ),
															   Data = response,
															   MomentType = MomentType.Response,
														   } );

			this.AddMap<Requests.CommandException>( docs => from doc in docs
															let error = doc.Error
															select new
															{
																doc.Id,
																doc.CreatedOn,
																AggregateIdentifier = "",
																doc.CorrelationId,
																doc.UserAccount,
																Type = AsDocument( error ).Value<string>( "$type" ),
																Data = error,
																MomentType = MomentType.Exception,
															} );

			this.StoreAllFields( FieldStorage.Yes );
		}
	}
}
