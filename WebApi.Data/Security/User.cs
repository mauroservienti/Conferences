using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Data.Security
{
	public class User : Aggregate
	{
		internal String Username { get; private set; }
		internal String FullName { get; private set; }

		private User()
		{

		}

		User SetupCompleted()
		{
			this.RaiseEvent( new Events.UserCreated( this.Id, this.Username, this.FullName ) );

			return this;
		}

		public class Factory
		{
			public User CreatePerson( string firstName, string lastName )
			{
				var person = new User()
				{
					//Id = "users/" + Guid.NewGuid().ToString(),
					FullName = firstName + " " + lastName,
					Username = lastName + firstName
				};

				return person.SetupCompleted();
			}
		}
	}
}
