using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using Raven.Client;
using Raven.Client.Linq;
using WebApi.Data.Indexes;
using WebApiServer.Models;
using WebApiServer;
using WebApiServer.Services;

namespace WebApiServer.Controllers.Api
{
	public class HistoryController : ApiController
	{
		readonly IDocumentStore store;
		readonly RestResourceFormatter resourceFormatter;

		public HistoryController( IDocumentStore documentStore, RestResourceFormatter resourceFormatter )
		{
			this.store = documentStore;
			this.resourceFormatter = resourceFormatter;
		}

		public dynamic Get( Int32 p = 0, Int32 s = 30 )
		{
			using ( var session = this.store.OpenSession() )
			{
				RavenQueryStatistics stats;
				var results = session.Query<History_Stream.Moment, History_Stream>()
					.Statistics( out stats )
					.Where( m => m.MomentType != History_Stream.MomentType.Response )
					.Skip( p * s )
					.Take( s )
					.OrderBy( m => m.CreatedOn )
					.ProjectFromIndexFieldsInto<History_Stream.Moment>()
					.ToList();

				var viewModel = new PagedResultsViewModel<History_Stream.Moment>()
				{
					PageIndex = p,
					PageSize = s,
					TotalPages = stats.TotalResults.ToPagesCount( s ),
					TotalResults = stats.TotalResults,
					IsStale = stats.IsStale,
					Results = results
				};

				var resource = this.resourceFormatter.AsResource( viewModel, this.Url );
				return resource;
			}
		}
	}
}
