using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Raven.Client;
using WebApi.Data.Indexes;
using WebApiServer.Models;
using Topics.Radical;
using WebApi.Data.Transformers;
using WebApi.Data.Views;
using WebApiServer.Services;

namespace WebApiServer.Controllers.Api
{
	public class SearchController : ApiController
	{
		readonly IDocumentStore store;
		readonly RestResourceFormatter resourceFormatter;

		public SearchController( IDocumentStore documentStore, RestResourceFormatter resourceFormatter )
		{
			this.store = documentStore;
			this.resourceFormatter = resourceFormatter;
		}

		public IEnumerable<String> GetSuggestions( String q )
		{
			using ( var session = this.store.OpenSession() )
			{
				var terms = q.AsRavenSearchTerms();

				var query = session.Query<Parties_Search_FullText.SearchMap, Parties_Search_FullText>()
					.Search( m => m.Content, terms, escapeQueryOptions: EscapeQueryOptions.AllowAllWildcards );

				var suggestions = query.Suggest();

				return suggestions.Suggestions;
			}
		}

		public dynamic Get( String q, Int32 p = 0, Int32 s = 10 )
		{
			using ( var session = this.store.OpenSession() )
			{
				var terms = q.AsRavenSearchTerms();

				RavenQueryStatistics stats;
				var query = session.Query<Parties_Search_FullText.SearchMap, Parties_Search_FullText>()
					.Statistics( out stats )
					.Search( m => m.Content, terms, escapeQueryOptions: EscapeQueryOptions.AllowAllWildcards )
					.Skip( p * s )
					.Take( s )
					.OrderByScore()
					.ProjectFromIndexFieldsInto<Parties_Search_FullText.SearchResult>();

				var suggestions = query.SuggestLazy();
				var results = query.ToList();

				var viewModel = new SearchResultsViewModel<Parties_Search_FullText.SearchResult>()
				{
					Query = q,
					PageIndex = p,
					PageSize = s,
					TotalPages = stats.TotalResults.ToPagesCount( s ),
					TotalResults = stats.TotalResults,
					IsStale = stats.IsStale,
					Results = results,
					Suggestions = suggestions.Value.Suggestions
				};

				var resourse = this.resourceFormatter.AsResource( viewModel, this.Url );

				return resourse;
			}
		}
	}

	public static class SearchExtensions
	{
		public static String AsRavenSearchTerms( this String userQuery )
		{
			if ( String.IsNullOrWhiteSpace( userQuery ) )
			{
				return "*";
			}

			var keywords = userQuery.ToLower().AsKeywords( ' ' ).ToArray();
			var terms = "";
			if ( keywords.Length == 1 )
			{
				terms = keywords[ 0 ];
			}
			else
			{
				terms = String.Format( "<<{0}>>", String.Join( " ", keywords ) );
			}

			return terms;
		}
	}

	public static class PagingHelper
	{
		public static Int32 ToPagesCount( this Int32 totalItemsCount, Int32 pageSize )
		{
			Int32 rem;
			var pagesCount = Math.DivRem( totalItemsCount, pageSize, out rem );
			if ( rem > 0 )
			{
				pagesCount++;
			}

			return pagesCount;
		}
	}
}
