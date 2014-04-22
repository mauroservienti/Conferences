using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;
using Topics.Radical.ComponentModel;

namespace WebApiServer.ComponentModel
{
	[Contract]
	public interface IResourceFormatter
	{
		dynamic AsResource( Object obj, UrlHelper urlHelper, Func<Object, IResourceFormatter> subformatterFinder );
		Boolean CanHandleObject( Object obj );
	}
}