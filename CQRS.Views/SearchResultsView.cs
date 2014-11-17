using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS.Views
{
	public class SearchResultsView<TResult> : PagedResultsView<TResult>
	{
		public IEnumerable<String> Suggestions { get; set; }
	}
}