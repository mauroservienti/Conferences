using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Views
{
	public class SearchResult
	{
		public String Id { get; internal set; }
		public String DisplayName { get; internal set; }

		public String EntityType { get; internal set; }
	}
}
