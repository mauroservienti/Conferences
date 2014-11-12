using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDBApplication.Model
{
	public class Order
	{
		public String Id { get; private set; }

		public Customer Customer { get; set; }
	}

	public class Customer 
	{
		public String Id { get; set; }
		public String Name { get; set; }

		public string Type { get; set; }
	}
}
