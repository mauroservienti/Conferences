using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Commands;
using WebApi.Data;
using WebApi.Data.Companies;

namespace WebApi.Backend.Handlers
{
	class CreateNewCompanyCommandHandler : NServiceBus.IHandleMessages<CreateNewCompanyCommand>
	{
		public IRepositoryFactory RepositoryFactory { get; set; }
		public Company.Factory CompanyFactory { get; set; }

		public void Handle( CreateNewCompanyCommand message )
		{
			using ( var repository = this.RepositoryFactory.OpenSession() ) 
			{
				var company = this.CompanyFactory.CreateCompany( message.CompanyName );
				
				repository.Save( company );
				repository.CommitChanges();
			}
		}
	}
}
