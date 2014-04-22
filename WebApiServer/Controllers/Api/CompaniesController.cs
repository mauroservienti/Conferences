using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Raven.Client;
using WebApi.Data.Companies;
using WebApi.Data.Transformers;
using WebApi.Data.Views;
using WebApiServer.Models;
using WebApiServer.Services;

namespace WebApiServer.Controllers.Api
{
	public class CompaniesController : ApiController
	{
		readonly IDocumentStore store;
		readonly RestResourceFormatter resourceFormatter;

		public CompaniesController( IDocumentStore documentStore, RestResourceFormatter resourceFormatter )
		{
			this.store = documentStore;
			this.resourceFormatter = resourceFormatter;
		}

		public dynamic Get( Int32 p = 0, Int32 s = 10 ) 
		{
			using ( var session = this.store.OpenSession() ) 
			{
				RavenQueryStatistics stats;
				var results = session.Query<Company>()
					.Statistics( out stats )
					.TransformWith<Company_CompanyView_Transformer, CompanyView>()
					.Skip( p * s )
					.Take( s )
					.ToList();

				var viewModel = new PagedResultsViewModel<CompanyView>()
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
			id = this.store.Conventions.FindTypeTagName( typeof( Company ) ) + "/" + id;
			using ( var session = this.store.OpenSession() )
			{
				var item = session.Load<Company_CompanyView_Transformer, CompanyView>( id );

				return this.resourceFormatter.AsResource( item, this.Url );
			}
		}
	}
}
