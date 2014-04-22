using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Commands;
using WebApi.Data;
using WebApi.Data.People;

namespace WebApi.Backend.Handlers
{
	class CreateNewPersonCommandHandler : NServiceBus.IHandleMessages<CreateNewPersonCommand>
	{
		public IRepositoryFactory RepositoryFactory { get; set; }
		public Person.Factory PersonFactory { get; set; }

		public void Handle( CreateNewPersonCommand message )
		{
			using ( var repository = this.RepositoryFactory.OpenSession() ) 
			{
				var person = this.PersonFactory.CreatePerson( message.FirstName, message.LastName );
				
				repository.Save( person );
				repository.CommitChanges();
			}
		}
	}
}
