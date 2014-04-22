using System;
using System.Collections.Concurrent;
using System.Threading;
using Raven.Abstractions.Commands;
using Raven.Abstractions.Data;
using Raven.Json.Linq;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Helpers;
using Topics.Radical.Validation;

namespace WebApi.Data.Runtime
{
	abstract class AbstractDispatchConsumer<T> where T: class
	{
		protected AbstractDispatchConsumer()
		{
			
		}

		protected abstract void Dispatch( T item );

		public virtual void ScheduleDispatch( T item )
		{
			Ensure.That( item ).Named( () => item ).IsNotNull();

			this.Dispatch( item );
		}
	}
}
