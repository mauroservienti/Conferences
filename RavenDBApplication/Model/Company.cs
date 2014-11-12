using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDBApplication.Model
{
	public class Company
	{
		public String Id { get; private set; }
		public String Name { get; set; }

		public static implicit operator Customer( Company source )
		{
			return new Customer()
			{
				Id = source.Id,
				Name = source.Name,
				Type = "Companies"
			};
		}
	}
}
