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
using RavenDBApplication.Indexes;
using RavenDBApplication.Model;

namespace RavenDBApplication
{
	class Program
	{
		static void Main( string[] args )
		{
			var store = Server.CreateDocumentStore();

			#region search people via linq

			using( var session = store.OpenSession() )
			{
				var notMoreThanTenPeople = session.Query<Person>()
					.Where( p => p.LastName.StartsWith( "S" ) || p.LastName.StartsWith( "M" ) )
					.Take( 10 )
					.ToList();
			}

			#endregion

			#region full text query

			using( var session = store.OpenSession() )
			{
				var searchResults = session.Query<FullText_Search.SearchResult, FullText_Search>()
					.Search( r => r.Content, "ma*", escapeQueryOptions: EscapeQueryOptions.AllowAllWildcards )
					.Take( 10 )
					.ProjectFromIndexFieldsInto<FullText_Search.SearchResult>()
					.ToList();
			}

			#endregion

			#region dynamic fields search

			using( var session = store.OpenSession() )
			{
				var query = session.Advanced.DocumentQuery<Product, Product_Search>()
					.Where( "Size: [40 TO 44]" )
					.AddOrder( "Size_Range", true, typeof( int ) );

				foreach( var item in query )
				{
					Console.WriteLine( "Product: {0} -> {1}", item.Name, item.Attributes.Single( a => a.Name == "Size" ).Value );
				}
			}

			#endregion
		}
	}
};
