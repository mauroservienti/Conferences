using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Data.Companies.Events
{
	public class CompanyCreated : DomainEvent
	{
		//[JsonConstructor]
		public CompanyCreated() { }

		public CompanyCreated( String aggregateId, string companyName )
			: base( aggregateId )
		{
			this.CompanyName = companyName;
		}

		public string CompanyName { get; private set; }
	}
}
