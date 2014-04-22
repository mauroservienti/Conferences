using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Indexes;
using WebApi.Data.People;

namespace WebApi.Data.Transformers
{
	public class Person_PersonView_Transformer : AbstractTransformerCreationTask<Person>
	{
		public Person_PersonView_Transformer()
		{
			this.TransformResults = results => from result in results
											   select new Views.PersonView()
											   {
												   Id = result.Id,
												   FirstName = result.FirstName,
												   LastName = result.LastName
											   };
		}
	}
}
