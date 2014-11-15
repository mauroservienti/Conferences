using Jason.Handlers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS.Commands.Handlers
{
	public class CreateNewPersonHandler : AbstractCommandHandler<CreateNewPerson>
	{
		protected override object OnExecute( CreateNewPerson command )
		{
			return Jason.Defaults.Response.Ok;
		}
	}
}