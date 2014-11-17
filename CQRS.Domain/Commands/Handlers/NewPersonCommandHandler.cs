using CQRS.Model.Domain;
using Jason.Handlers.Commands;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS.Commands.Handlers
{
	public class NewPersonCommandHandler : AbstractCommandHandler<NewPersonCommand>
	{
		public IDocumentSession Session { get; set; }

		protected override object OnExecute( NewPersonCommand command )
		{
			var person = Person.Create( command.FirstName, command.LastName );
			this.Session.Store( person );

			return Jason.Defaults.Response.Ok;
		}
	}
}