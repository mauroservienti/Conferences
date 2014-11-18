using CQRS.Commands.Handlers;
using CQRS.Model.Domain;
using CQRS.Views;
using Jason.WebAPI.Filters;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CQRS.Controllers
{
	public class CompaniesController : ApiController
	{
		IDocumentSession session;

		public CompaniesController( IDocumentSession session )
		{
			this.session = session;
		}

		public CompanyView Get( int id )
		{
			var view = this.session.LoadCompanyViewById( "Companies/" + id );
			return view;
		}

		public PagedResultsView<CompanyView> Get( int p = 0, int s = 10 )
		{
			var view = this.session.GetCompanyViews( p, s );

			return view;
		}
	}
}
