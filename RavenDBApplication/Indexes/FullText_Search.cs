using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace RavenDBApplication.Indexes
{
	public class FullText_Search : AbstractMultiMapIndexCreationTask<FullText_Search.SearchResult>
	{
		public class SearchResult
		{
			public String Id { get; set; }
			public String Description { get; set; }
			public String Type { get; set; }

			public Object[] Content { get; set; }
		}

		public FullText_Search()
		{
			this.AddMap<Model.Person>( docs => from doc in docs
											   select new SearchResult()
											   {
												   Id = doc.Id,
												   Description = doc.FirstName + " " + doc.LastName,
												   Type = "People",
												   Content = new Object[] 
												   {
													   doc.FirstName,
													   doc.LastName
												   }
											   } );

			this.AddMap<Model.Company>( docs => from doc in docs
												select new SearchResult()
												{
													Id = doc.Id,
													Description = doc.Name,
													Type = "Companies",
													Content = new Object[] 
												   {
													   doc.Name
												   }
												} );

			this.AddMap<Model.Order>( docs => from doc in docs
											  select new SearchResult()
											  {
												  Id = doc.Id,
												  Description = "Order: " + doc.Id + " for " + doc.Customer.Name,
												  Type = "Orders",
												  Content = new Object[] 
												   {
													   doc.Customer.Name
												   }
											  } );

			this.AddMap<Model.Product>( docs => from doc in docs
												select new SearchResult()
												{
													Id = doc.Id,
													Description = "Product: " + doc.Id + " for " + doc.Name,
													Type = "Orders",
													Content = new Object[] 
												  {
													doc.Name,
													doc.Attributes.Select(a=>a.FieldValue).ToArray()
												  }
												} );

			this.StoreAllFields( FieldStorage.Yes );
		}
	}
}
