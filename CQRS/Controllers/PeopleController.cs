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
	public class PeopleController : ApiController
	{
		IDocumentSession session;

		public PeopleController(IDocumentSession session)
		{
			this.session = session;
		}

		[HttpPost]
		public HttpStatusCode CreateNewPerson(String firstName, String lastName)
		{
			return HttpStatusCode.OK;
		}

		[HttpPost]
		public HttpStatusCode CreatePerson(Commands.NewPersonCommand command)
		{
			var handler = new NewPersonCommandHandler();
			handler.Session = this.session;

			handler.Execute(command);

			return HttpStatusCode.OK;
		}





















		[InterceptCommandAction(HttpStatusCode.OK)]
		public void Post(Commands.CreateNewPerson payload)
		{

		}

















		public PersonView Get(int id)
		{
			var view = this.session.LoadPersonViewById("People/" + id);
			return view;
		}
	}
}
