using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Data.People.Events
{
	public class PersonCreated : DomainEvent
	{
		//[JsonConstructor]
		public PersonCreated() { }

		public PersonCreated( String aggregateId, string firstName, string lastName )
			: base( aggregateId )
		{
			this.FirstName = firstName;
			this.LastName = lastName;
		}

		public string LastName { get; private set; }
		public string FirstName { get; private set; }
	}
}
