using CQRS.Views;
using CQRS.Views.Specifications;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CQRS.Controllers
{
    public class SearchController : ApiController
    {
		IDocumentStore store;

		public SearchController( IDocumentStore store )
		{
			this.store = store;
		}
		public SearchResultsView<SearchResult> Get( String q, int p = 0, int s = 10 ) 
		{
			var query = new FullTextQuery( this.store ) 
			{
				PageIndex = p,
				PageSize = s
			};
			var results = query.Execute( q );

			return results;
		}
    }
}
