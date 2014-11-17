using CQRS.Model.Domain;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Views.Indexes
{
	class Person_PersonView_Transformer : AbstractTransformerCreationTask<Person>
	{
		public Person_PersonView_Transformer()
		{
			this.TransformResults = docs => from doc in docs
											select new PersonView()
											{
												FullName = doc.FirstName + " " + doc.LastName,
												Id = doc.Id
											};
		}
	}
}
