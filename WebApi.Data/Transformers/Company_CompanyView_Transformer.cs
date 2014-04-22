using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Indexes;
using WebApi.Data.Companies;

namespace WebApi.Data.Transformers
{
	public class Company_CompanyView_Transformer : AbstractTransformerCreationTask<Company>
	{
		public Company_CompanyView_Transformer()
		{
			this.TransformResults = results => from result in results
											   select new Views.CompanyView()
											   {
												   Id = result.Id,
												   CompanyName = result.CompanyName
											   };
		}
	}
}
