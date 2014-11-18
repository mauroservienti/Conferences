using CQRS.Model.Domain;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Views
{
	public class PersonView
	{
		internal PersonView()
		{

		}

		public String Id { get; internal set; }
		public String FullName { get; internal set; }
	}

	public static class PersonViewExtensions 
	{
		public static PersonView LoadPersonViewById( this IDocumentSession session, String id ) 
		{
			var view = session.Load<Indexes.Person_PersonView_Transformer, PersonView>( id );

			return view;
		}

		public static PagedResultsView<PersonView> GetPersonViews( this IDocumentSession session, Int32 pageIndex, Int32 pageSize )
		{
			RavenQueryStatistics stats;
			var query = session.Query<Person>()
				.Statistics( out stats )
				.Customize( c =>
				{
					c.NoTracking();
				} )
				.TransformWith<Indexes.Person_PersonView_Transformer, PersonView>()
				.Skip( pageIndex * pageSize )
				.Take( pageSize );

			var results = query.ToList();

			var viewModel = new PagedResultsView<PersonView>()
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
