using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Data.Companies
{
	public class Company : Aggregate
	{
		internal String CompanyName { get; private set; }

		private Company()
		{

		}

		Company SetupCompleted()
		{
			this.RaiseEvent( new Events.CompanyCreated( this.Id, this.CompanyName ) );

			return this;
		}

		public class Factory 
		{
			public Company CreateCompany( string companyName )
			{
				var company = new Company()
				{
					//Id = "companies/" + Guid.NewGuid().ToString(),
					CompanyName = companyName
				};

				return company.SetupCompleted();
			}
		}
	}
}
