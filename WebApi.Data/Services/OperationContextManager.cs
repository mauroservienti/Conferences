using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Topics.Radical.Validation;
using Topics.Radical;
using WebApi.Infrastructure;

namespace WebApi.Data.Services
{
	class OperationContextManager : IOperationContextManager
	{
		readonly IServiceProvider container;

		public OperationContextManager( IServiceProvider container )
		{
			Ensure.That( container ).Named( () => container ).IsNotNull();

			this.container = container;
		}

		public IOperationContext GetCurrent()
		{
			return this.container.GetService<IOperationContext>();
		}
	}
}