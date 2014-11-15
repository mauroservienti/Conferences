using Jason.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CQRS.Controllers
{
    public class SampleController : ApiController
    {
		public Object Get() 
		{
			return null;
		}

		[InterceptCommandAction]
		public void Post( Commands.CreateNewPerson payload ) 
		{
 
		}
    }
}
