using CQRS.Views.Indexes;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Views.Specifications
{
	public class FullTextQuery
	{
		IDocumentStore store;

		public FullTextQuery( IDocumentStore store )
		{
			this.store = store;

			this.PageSize = 10;
			this.PageIndex = 0;
		}

		public int PageSize { get; set; }

		public int PageIndex { get; set; }

		public SearchResultsView<SearchResult> Execute( String queryText )
		{
			using( var session = this.store.OpenSession() )
			{
				var terms = queryText.AsRavenSearchTerms();

				RavenQueryStatistics stats;
				var query = session.Query<FullText_Search.SearchMap, FullText_Search>()
					.Statistics( out stats )
					.Customize( c =>
					{
						c.NoTracking();
					} )
					.Search( m => m.Content, terms, escapeQueryOptions: EscapeQueryOptions.AllowAllWildcards )
					.ProjectFromIndexFieldsInto<SearchResult>()
					.OrderByScore()
					.Skip( this.PageIndex * this.PageSize )
					.Take( this.PageSize );

				var suggestions = query.SuggestLazy();
				var results = query.ToList();

				var viewModel = new SearchResultsView<SearchResult>()
				{
					PageIndex = this.PageIndex,
					PageSize = this.PageSize,
					TotalPages = stats.TotalResults.ToPagesCount( this.PageSize ),
					TotalResults = stats.TotalResults,
					IsStale = stats.IsStale,
					Results = results,
					Suggestions = suggestions.Value.Suggestions
				};

				return viewModel;
			}
		}
	}
}
