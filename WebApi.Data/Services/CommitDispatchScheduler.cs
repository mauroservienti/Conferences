using System;
using System.Linq;
using System.Collections.Generic;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Validation;
using Raven.Client;
using WebApi.Data.Runtime;
using NServiceBus;
using WebApi.Infrastructure;
using Topics.Radical.Bootstrapper;

namespace WebApi.Data.Services
{
	class CommitDispatchScheduler : AbstractDispatchScheduler<Commits.Commit, CommitDispatchConsumer>, ICommitDispatchScheduler, IRequireToStart
	{
		readonly IOperationContextManager contextManager;
		readonly IBus bus;
		readonly IDocumentStore store;

		public CommitDispatchScheduler( IDocumentStore store, IBus bus, IOperationContextManager contextManager )
			: base( store )
		{
			this.store = store;
			this.contextManager = contextManager;
			this.bus = bus;
		}

		protected override CommitDispatchConsumer CreateConsumer()
		{
			var consumer = new CommitDispatchConsumer( this.bus, this.store, this.contextManager );

			return consumer;
		}

		public void Start()
		{
			this.TryResume();
		}

		public bool IsSynchronous
		{
			get { return true; }
		}
	}
}
