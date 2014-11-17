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
	}
}
