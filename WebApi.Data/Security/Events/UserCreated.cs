using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Data.Security.Events
{
	public class UserCreated : DomainEvent
	{
		//[JsonConstructor]
		public UserCreated() { }

		public UserCreated( String aggregateId, string fullName, string username )
			: base( aggregateId )
		{
			this.FullName = fullName;
			this.Username = username;
		}

		public string Username { get; private set; }
		public string FullName { get; private set; }
	}
}
