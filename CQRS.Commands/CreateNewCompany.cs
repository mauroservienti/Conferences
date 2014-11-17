using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CQRS.Commands
{
	public class CreateNewCompany
	{
		[Required]
		public String CompanyName { get; set; }

		[Required, StringLength( 10, MinimumLength = 10 )]
		public String VatNumber { get; set; }
	}
}