using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS.Views
{
	public class PagedResultsView<TResult>
	{
		public IEnumerable<TResult> Results { get; set; }

		public int TotalResults { get; set; }

		public bool IsStale { get; set; }

		public int PageSize { get; set; }

		public int PageIndex { get; set; }

		public int TotalPages { get; set; }
	}
}