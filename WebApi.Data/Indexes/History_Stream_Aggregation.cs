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
	public class History_Stream_Aggregation : AbstractMultiMapIndexCreationTask<History_Stream_Aggregation.MomentCloud>
	{
		public enum MomentType
		{
			Event,
			Command,
			Response,
			Exception
		}

		public class MomentCloud
		{
			public Int32 Count { get; private set; }
			public MomentType MomentType { get; private set; }
		}

		public History_Stream_Aggregation()
		{
			this.AddMap<Commit>( docs => from doc in docs
										 from evt in doc.Events
										 select new
										 {
											 Count = 1,
											 MomentType = MomentType.Event,
										 } );

			this.AddMap<Requests.CommandRequest>( docs => from doc in docs
														  let command = doc.Command
														  select new
														  {
															  Count = 1,
															  MomentType = MomentType.Command,
														  } );

			this.AddMap<Requests.CommandResponse>( docs => from doc in docs
														   let response = doc.Response
														   select new
														   {
															   Count = 1,
															   MomentType = MomentType.Response,
														   } );

			this.AddMap<Requests.CommandException>( docs => from doc in docs
															let error = doc.Error
															select new
															{
																Count = 1,
																MomentType = MomentType.Exception,
															} );

			this.Reduce = results => from result in results
									 group result by result.MomentType into g
									 select new
									 {
										 MomentType = g.Key,
										 Count = g.Sum( e => e.Count )
									 };

			this.StoreAllFields( FieldStorage.Yes );
		}
	}
}
