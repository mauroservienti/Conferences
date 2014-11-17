using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Views
{
	public static class PagingHelper
	{
		public static Int32 ToPagesCount( this Int32 totalItemsCount, Int32 pageSize )
		{
			Int32 rem;
			var pagesCount = Math.DivRem( totalItemsCount, pageSize, out rem );
			if( rem > 0 )
			{
				pagesCount++;
			}

			return pagesCount;
		}
	}
}
