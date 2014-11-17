using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;

namespace CQRS.Views
{
	public static class SearchExtensions
	{
		public static String AsRavenSearchTerms( this String userQuery )
		{
			var keywords = userQuery.ToLower().AsKeywords( ' ' ).ToArray();
			var terms = "";
			if( keywords.Length == 1 )
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
}
