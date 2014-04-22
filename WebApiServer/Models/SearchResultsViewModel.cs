using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiServer.Models
{
	public class SearchResultsViewModel<TResult> : PagedResultsViewModel<TResult>
	{
		public String Query { get; set; }
		public IEnumerable<String> Suggestions { get; set; }
	}
}