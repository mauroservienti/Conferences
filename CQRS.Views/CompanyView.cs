using CQRS.Model.Domain;
using Raven.Client;
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

	public static class CompanyViewExtensions
	{
		public static CompanyView LoadCompanyViewById( this IDocumentSession session, String id )
		{
			var view = session.Load<Indexes.Company_CompanyView_Transformer, CompanyView>( id );

			return view;
		}

		public static PagedResultsView<CompanyView> GetCompanyViews( this IDocumentSession session, Int32 pageIndex, Int32 pageSize )
		{
			RavenQueryStatistics stats;
			var query = session.Query<Company>()
				.Statistics( out stats )
				.Customize( c =>
				{
					c.NoTracking();
				} )
				.TransformWith<Indexes.Company_CompanyView_Transformer, CompanyView>()
				.Skip( pageIndex * pageSize )
				.Take( pageSize );

			var results = query.ToList();

			var viewModel = new PagedResultsView<CompanyView>()
			{
				PageIndex = pageIndex,
				PageSize = pageSize,
				TotalPages = stats.TotalResults.ToPagesCount( pageSize ),
				TotalResults = stats.TotalResults,
				IsStale = stats.IsStale,
				Results = results
			};

			return viewModel;
		}
	}
}
