
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Topics.Radical.Validation;

namespace CQRS.Model.Domain
{
	public class Person
	{
		internal static Person Create( string firstName, string lastName )
		{
			Ensure.That( firstName ).Named( () => firstName ).IsNotNullNorEmpty();
			Ensure.That( lastName ).Named( () => lastName ).IsNotNullNorEmpty();

			var person = new Person() 
			{
				FirstName =  firstName,
				LastName =  lastName
			};

			return person;
		}

		private Person() 
		{

		}

		public String Id { get; private set; }

		internal string LastName { get; private set; }

		internal string FirstName { get; private set; }
	}
}