using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Data.Views
{
	public class PersonView
	{
		public string FirstName { get; internal set; }
		public string LastName { get; internal set; }

		public string Id { get; internal set; }
	}
}
