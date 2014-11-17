using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CQRS.Commands
{
	public class NewPersonCommand
	{
		public String FirstName { get; set; }

		public String LastName { get; set; }
	}
}