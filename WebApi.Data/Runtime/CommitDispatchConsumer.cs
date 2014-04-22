using System;
using NServiceBus;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Json.Linq;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Helpers;
using Topics.Radical.Validation;
using WebApi.Infrastructure;

namespace WebApi.Data.Runtime
{
	class CommitDispatchConsumer : AbstractDispatchConsumer<Commits.Commit>
	{
		readonly IOperationContextManager contextManager;
		readonly IBus bus;
		readonly IDocumentStore store;

		public CommitDispatchConsumer( IBus bus, IDocumentStore store, IOperationContextManager contextManager )
		{
			Ensure.That( bus ).Named( () => bus ).IsNotNull();
			Ensure.That( store ).Named( () => store ).IsNotNull();
			Ensure.That( contextManager ).Named( () => contextManager ).IsNotNull();

			this.bus = bus;
			this.store = store;
			this.contextManager = contextManager;
		}

		protected override void Dispatch( Commits.Commit commit )
		{
			foreach ( var @event in commit.Events )
			{
				this.bus.Publish( @event );
			}
		}
	}
}
