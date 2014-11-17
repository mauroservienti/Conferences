using CQRS.Model.Domain;
using Jason.Handlers.Commands;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS.Commands.Handlers
{
	public class CreateNewCompanyHandler : AbstractCommandHandler<CreateNewCompany>
	{
		public IDocumentSession Session { get; set; }

		protected override object OnExecute( CreateNewCompany command )
		{
			var company = Company.Create( command.CompanyName, command.VatNumber );
			this.Session.Store( company );

			return Jason.Defaults.Response.Ok;
		}
	}
}