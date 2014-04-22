using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace WebApiServer.Infrastructure
{
	public class DelegateDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
	{
		public DelegateDependencyResolver()
		{
			this.OnBeginScope = () => this;
			this.OnGetService = t => null;
			this.OnGetServices = t => new List<Object>();
			this.OnDispose = () => { };
		}

		public Func<IDependencyScope> OnBeginScope { get; set; }

		public IDependencyScope BeginScope()
		{
			return this.OnBeginScope();
		}

		public Func<Type, Object> OnGetService { get; set; }

		public object GetService( Type serviceType )
		{
			return this.OnGetService( serviceType );
		}

		public Func<Type, IEnumerable<Object>> OnGetServices { get; set; }

		public IEnumerable<object> GetServices( Type serviceType )
		{
			return this.OnGetServices( serviceType );
		}

		public Action OnDispose { get; set; }

		public void Dispose()
		{
			this.OnDispose();
		}
	}
}