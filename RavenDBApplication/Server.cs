using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Shard;

namespace RavenDBApplication
{
	static class Server
	{
		public static IDocumentStore CreateDocumentStore()
		{
			var shards = new Dictionary<String, IDocumentStore>()
			{
				{
					"S01", 
					new DocumentStore()
					{
						Url = "http://localhost:8381",
						DefaultDatabase = "Sample"
					}.Initialize()
				},
				{
					"S02", 
					new DocumentStore()
					{
						Url = "http://localhost:8382",
						DefaultDatabase = "Sample"
					}.Initialize()
				},
				{
					"S03", 
					new DocumentStore()
					{
						Url = "http://localhost:8383",
						DefaultDatabase = "Sample"
					}.Initialize()
				}
			};

			var strategy = new ShardStrategy( shards );


			strategy.ShardingOn<Model.Company>();
			strategy.ShardingOn<Model.Person>();




			strategy.ShardingOn<Model.Product>( p => p.Id, id => "S03" );
			strategy.ShardingOn<Model.Order>( o => o.Customer.Type, t =>
			{
				if ( t == "People" )
				{
					return "S01";
				}

				return "S02";
			} );

			var store = new ShardedDocumentStore( strategy );

			store.Initialize();

			IndexCreation.CreateIndexes( Assembly.GetEntryAssembly(), store );

			return store;
		}
	}
}
