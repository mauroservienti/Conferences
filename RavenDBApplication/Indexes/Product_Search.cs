using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Indexes;
using RavenDBApplication.Model;

namespace RavenDBApplication.Indexes
{
	public class Product_Search : AbstractIndexCreationTask<Product>
	{
		public Product_Search()
		{
			this.Map = docs => from doc in docs
							   select new
							   {
								   doc.Name,
								   _ = doc.Attributes.Select( a => CreateField( a.Name, a.FieldValue, false, true ) )
							   };
		}
	}
}
