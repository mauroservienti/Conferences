using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDBApplication.Model
{
	public class Person
	{
		public String Id { get; private set; }

		public String FirstName { get; set; }

		public String LastName { get; set; }

		public static implicit operator Customer( Person source ) 
		{
			return new Customer() 
			{
				Id = source.Id,
				Name = source.FirstName + " " + source.LastName,
				Type = "People"
			};
		}
	}
}
