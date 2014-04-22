using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Validation;

namespace WebApi.Data.People
{
	public class Person : Aggregate
	{
		internal String FirstName { get; private set; }
		internal String LastName { get; private set; }

		internal string Address { get; set; }

		private Person()
		{

		}

		//public void AddPersonAddress( String address ) 
		//{
		//	Ensure.That( address ).Named( () => address ).IsNotNullNorEmpty();

		//	this.Address = address;

		//	this.RaiseEvent( new AddressAddedToPerson( this.Id, address ) );
		//} 

		Person SetupCompleted()
		{
			this.RaiseEvent( new Events.PersonCreated( this.Id, this.FirstName, this.LastName ) );

			return this;
		}

		public class Factory 
		{
			public Person CreatePerson( string firstName, string lastName )
			{
				var person = new Person()
				{
					//Id = "people/" + Guid.NewGuid().ToString(),
					FirstName = firstName,
					LastName = lastName
				};

				person.SetupCompleted();

				return person;
			}
		}

	}
}
