using CQRS.Model.Domain;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Views.Indexes
{
	public class FullText_Search : AbstractMultiMapIndexCreationTask<FullText_Search.SearchMap>
	{
		public class SearchMap
		{
			public string[] Content { get; internal set; }
			public string DisplayName { get; internal set; }

			public string Id { get; internal set; }

			public String EntityType { get; set; }
		}

		public FullText_Search()
		{
			this.AddMap<Person>( docs => from doc in docs
										 select new SearchMap
										 {
											 Id = doc.Id,
											 EntityType = this.MetadataFor( doc )[ "Raven-Entity-Name" ].ToString(),
											 Content = new[] 
											 {
												 doc.FirstName,
												 doc.LastName
											 },
											 DisplayName = doc.FirstName + " " + doc.LastName
										 } );

			this.AddMap<Company>( docs => from doc in docs
										 select new SearchMap
										 {
											 Id = doc.Id,
											 EntityType = this.MetadataFor( doc )[ "Raven-Entity-Name" ].ToString(),
											 Content = new[] 
											 {
												 doc.Name,
												 doc.VatNumber
											 },
											 DisplayName = doc.Name + " (" + doc.VatNumber + ")"
										 } );

			this.StoreAllFields( FieldStorage.Yes );
		}
	}
}
