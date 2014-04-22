using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiServer.Controllers.Api
{
	public class RootController : ApiController
	{
		public dynamic Get()
		{
			var endpoints = new List<dynamic>();

			dynamic search = new ExpandoObject();
			search.Name = "Search";
			search.Description = "Query for resources";
			search.RefTemplate = this.Url.Link( "DefaultApiGet", new
			{
				controller = "Search",
				q = "--q-",
				p = "--p-",
				s = "--s-"
			} )
			.Replace( "--", "{" )
			.Replace( "-", "}" );

			endpoints.Add( search );

			dynamic people = new ExpandoObject();
			people.Name = "People";
			people.Description = "People management";
			people.RefTemplate = this.Url.Link( "DefaultApiGet", new
			{
				controller = "People",
				p = "--p-",
				s = "--s-"
			} )
			.Replace( "--", "{" )
			.Replace( "-", "}" );

			dynamic person = new ExpandoObject();
			person.Name = "Person";
			person.Description = "Person management";
			person.RefTemplate = this.Url.Link( "DefaultApiWithId", new
			{
				controller = "People",
				id = "12"
			} )
			.Replace( "12", "{id}" );
			var personEndpoints = new List<dynamic>();
			personEndpoints.Add( person );
			people.Endpoints = personEndpoints;

			endpoints.Add( people );

			dynamic companies = new ExpandoObject();
			companies.Name = "Companies";
			companies.Description = "Companies management";
			companies.RefTemplate = this.Url.Link( "DefaultApiGet", new
			{
				controller = "Companies",
				p = "--p-",
				s = "--s-"
			} )
			.Replace( "--", "{" )
			.Replace( "-", "}" );

			dynamic company = new ExpandoObject();
			company.Name = "Company";
			company.Description = "Company management";
			company.RefTemplate = this.Url.Link( "DefaultApiWithId", new
			{
				controller = "Companies",
				id = "12"
			} )
			.Replace( "12", "{id}" );
			var companiesEndpoints = new List<dynamic>();
			companiesEndpoints.Add( company );
			companies.Endpoints = companiesEndpoints;

			endpoints.Add( companies );

			dynamic root = new ExpandoObject();
			root.Self = this.Url.Link( "DefaultApiGet", null );
			root.Name = "FullStack Sample API";
			root.Description = "This is the FullStack Sample API.";
			root.Endpoints = endpoints;

			return root;
		}
	}
}
