
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Raven.Client;
using WebApi.Data;
using WebApi.Data.Commits;
using WebApiServer.Services;

namespace WebApiServer.Controllers.Api
{
	public class CommitsController : ApiController
	{
		readonly IDocumentStore store;
		readonly RestResourceFormatter resourceFormatter;

		public CommitsController( IDocumentStore documentStore, RestResourceFormatter resourceFormatter )
		{
			this.store = documentStore;
			this.resourceFormatter = resourceFormatter;
		}

		public dynamic Get( String id )
		{
			using ( var session = this.store.OpenSession() )
			{
				id = this.store.Conventions.FindTypeTagName( typeof( Commit ) ) + "/" + id;
				var item = session.Load<Commit>( id );

				return this.resourceFormatter.AsResource( item, this.Url );
			}
		}

		public dynamic Get( String commitId, String eventId )
		{
			return "Commits/"+ commitId + "/events/" + eventId;
		}

		[HttpGet]
		public dynamic Events( String commitId )
		{
			using ( var session = this.store.OpenSession() )
			{
				//Avrebbe senso un transformer
				commitId = this.store.Conventions.FindTypeTagName( typeof( Commit ) ) + "/" + commitId;
				var item = session.Load<Commit>( commitId );

				var resource = this.resourceFormatter.AsResource( item.Events, this.Url );
				//resource.Self = this.Url.Link( "CommitsEventsApiWithId", new
				//{
				//	commitId = commitId
				//} );

				return resource;
			}
		}
	}
}
