using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CQRS.Commands
{
	public class CreateNewPerson
	{
		[Required]
		public String FirstName { get; set; }

		[Required]
		public String LastName { get; set; }
	}
}