using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Views
{
	public class CompanyView
	{
		internal CompanyView()
		{

		}

		public String Id { get; internal set; }
		public String Name { get; internal set; }
	}
}
