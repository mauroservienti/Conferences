using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using WebApi.Data.Companies;
using WebApi.Data.People;

namespace WebApi.Data.Indexes
{
	public class Parties_Search_FullText : AbstractMultiMapIndexCreationTask<Parties_Search_FullText.SearchResult>
	{
		public class SearchMap
		{
			public object[] Content { get; private set; }
		}

		public class SearchResult
		{
			public String Id { get; private set; }
			public String DisplayName { get; private set; }
			public String ClrType { get; private set; }
			public String Collection { get; private set; }
		}

		public Parties_Search_FullText()
		{
			this.AddMap<Person>( docs => from doc in docs
										 select new
										 {
											 Id = doc.Id,
											 Content = new object[] 
											 { 
												 doc.FirstName,
												 doc.LastName,
											 },
											 DisplayName = doc.FirstName + " " + doc.LastName,
											 ClrType = MetadataFor( doc ).Value<String>( "Raven-Clr-Type" ),
											 Collection = MetadataFor( doc ).Value<String>( "Raven-Entity-Name" ),
										 } );

			this.AddMap<Company>( docs => from doc in docs
										  select new
										  {
											  Id = doc.Id,
											  Content = new object[] 
											  { 
												  doc.CompanyName, 
											  },
											  DisplayName = doc.CompanyName,
											  ClrType = MetadataFor( doc ).Value<String>( "Raven-Clr-Type" ),
											  Collection = MetadataFor( doc ).Value<String>( "Raven-Entity-Name" ),
										  } );

			this.Store( r => r.Id, FieldStorage.Yes );
			this.Store( r => r.DisplayName, FieldStorage.Yes );
			this.Store( r => r.ClrType, FieldStorage.Yes );
			this.Store( r => r.Collection, FieldStorage.Yes );
		}
	}
}
