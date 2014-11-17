
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Topics.Radical.Validation;

namespace CQRS.Model.Domain
{
	internal class Company
	{
		internal static Company Create( string name, string vatNumber )
		{
			Ensure.That( name ).Named( () => name ).IsNotNullNorEmpty();
			Ensure.That( vatNumber ).Named( () => vatNumber )
				.IsNotNullNorEmpty()
				.IsTrue( s => s.Length == 10 );

			var company = new Company()
			{
				Name = name,
				VatNumber = vatNumber
			};

			return company;
		}

		private Company()
		{

		}

		public String Id { get; private set; }

		internal string Name { get; private set; }

		internal string VatNumber { get; private set; }
	}
}