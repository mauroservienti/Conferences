using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Raven.Client;
using WebApi.Data.People;
using WebApi.Data.Transformers;
using WebApi.Data.Views;
using WebApiServer.Models;
using WebApiServer.Services;

namespace WebApiServer.Controllers.Api
{
	public class PeopleController : ApiController
	{
		readonly IDocumentStore store;
		readonly RestResourceFormatter resourceFormatter;

		public PeopleController( IDocumentStore documentStore, RestResourceFormatter resourceFormatter )
		{
			this.store = documentStore;
			this.resourceFormatter = resourceFormatter;
		}

		public dynamic Get( Int32 p = 0, Int32 s = 10 )
		{
			using ( var session = this.store.OpenSession() )
			{
				RavenQueryStatistics stats;
				var results = session.Query<Person>()
					.Statistics( out stats )
					.TransformWith<Person_PersonView_Transformer, PersonView>()
					.Skip( p * s )
					.Take( s )
					.ToList();

				var viewModel = new PagedResultsViewModel<PersonView>()
				{
					PageIndex = p,
					PageSize = s,
					TotalPages = stats.TotalResults.ToPagesCount( s ),
					TotalResults = stats.TotalResults,
					IsStale = stats.IsStale,
					Results = results
				};

				var resourse = this.resourceFormatter.AsResource( viewModel, this.Url );

				return resourse;
			}
		}

		public dynamic Get( String id )
		{
			id = this.store.Conventions.FindTypeTagName( typeof( Person ) ) + "/" + id;
			using ( var session = this.store.OpenSession() )
			{
				var item = session.Load<Person_PersonView_Transformer, PersonView>( id );

				return this.resourceFormatter.AsResource( item, this.Url );
			}
		}
	}
}