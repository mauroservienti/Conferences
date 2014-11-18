using CQRS.Model.Domain;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Views.Indexes
{
	class Company_CompanyView_Transformer : AbstractTransformerCreationTask<Company>
	{
		public Company_CompanyView_Transformer()
		{
			this.TransformResults = docs => from doc in docs
											select new CompanyView()
											{
												Name = doc.Name + " (" +  doc.VatNumber + ")",
												Id = doc.Id
											};
		}
	}
}
