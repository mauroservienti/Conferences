using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Raven.Abstractions.Exceptions;
using Raven.Client;
using Topics.Radical.Linq;
using Topics.Radical.Validation;
using WebApi.Data.Commits;
using WebApi.Infrastructure;

namespace WebApi.Data.Services
{
	class RepositoryFactory : IRepositoryFactory
	{
		class Repository : IRepository
		{
			public void Dispose()
			{
				this.session.Dispose();

				this.aggregateTracking.Clear();
			}

			readonly IOperationContextManager contextManager;
			readonly ICommitDispatchScheduler commitDispatchScheduler;
			readonly IDocumentSession session;
			readonly HashSet<IAggregate> aggregateTracking = new HashSet<IAggregate>();
			readonly Guid txId;
			readonly Commit.Factory commitFactory;

			public Repository( IOperationContextManager contextManager, IDocumentSession session, ICommitDispatchScheduler commitDispatchScheduler, Commit.Factory commitFactory )
			{
				Ensure.That( contextManager ).Named( () => contextManager ).IsNotNull();
				Ensure.That( session ).Named( () => session ).IsNotNull();
				Ensure.That( commitDispatchScheduler ).Named( () => commitDispatchScheduler ).IsNotNull();
				Ensure.That( commitFactory ).Named( () => commitFactory ).IsNotNull();

				this.txId = Guid.NewGuid();

				this.contextManager = contextManager;
				this.session = session;
				this.session.Advanced.UseOptimisticConcurrency = true;
				this.commitDispatchScheduler = commitDispatchScheduler;
				this.commitFactory = commitFactory;
			}

			public void Save<TAggregate>( TAggregate aggregate ) where TAggregate : IAggregate
			{
				this.session.Store( aggregate );

				this.TrackIfRequired( aggregate );
			}

			public void CommitChanges()
			{
				try
				{
					var operationContext = this.contextManager.GetCurrent();
					var correlationId = operationContext.CorrelationId;

					var userAccount = Thread.CurrentPrincipal.Identity.Name;

					var commits = this.aggregateTracking
						.Where( a => a.IsChanged )
						.Select( aggregate => new
						{
							Aggregate = aggregate,
							Commit = this.commitFactory.CreateFor( this.txId, correlationId, aggregate, userAccount )
						} )
						.ToArray()
						.ForEach( temp =>
						{
							var aggregate = temp.Aggregate;
							var commit = temp.Commit;

							if ( this.commitDispatchScheduler.IsSynchronous )
							{
								commit.MarkAsDispatched();
							}

							this.session.Store( commit );
						} )
						.Select( temp => temp.Commit )
						.ToArray();

					this.session.SaveChanges();

					this.aggregateTracking.ForEach( a => a.ClearUncommittedEvents() );
					this.aggregateTracking.Clear();

					this.commitDispatchScheduler.ScheduleDispatch( commits );
				}
				catch ( ConcurrencyException cex )
				{
					//TODO: log
					throw;
				}
			}

			void TrackIfRequired( IAggregate aggregate )
			{
				if ( !this.aggregateTracking.Contains( aggregate ) )
				{
					this.aggregateTracking.Add( aggregate );
				}
			}

			public TAggregate GetById<TAggregate>( string aggregateId ) where TAggregate : IAggregate
			{
				var aggregate = this.session.Load<TAggregate>( aggregateId );
				this.TrackIfRequired( aggregate );

				return aggregate;
			}

			public TAggregate[] GetById<TAggregate>( params string[] aggregateIds ) where TAggregate : IAggregate
			{
				var aggregates = this.session.Load<TAggregate>( aggregateIds );
				foreach ( var a in aggregates )
				{
					this.TrackIfRequired( a );
				}

				return aggregates;
			}
		}

		readonly IOperationContextManager contextManager;
		readonly ICommitDispatchScheduler commitDispatcher;
		readonly IDocumentStore store;
		readonly Commit.Factory commitFactory;

		public RepositoryFactory( IOperationContextManager contextManager, IDocumentStore store, ICommitDispatchScheduler commitDispatcher, Commit.Factory commitFactory )
		{
			Ensure.That( contextManager ).Named( () => contextManager ).IsNotNull();
			Ensure.That( store ).Named( () => store ).IsNotNull();
			Ensure.That( commitDispatcher ).Named( () => commitDispatcher ).IsNotNull();
			Ensure.That( commitFactory ).Named( () => commitFactory ).IsNotNull();

			this.contextManager = contextManager;
			this.store = store;
			this.commitDispatcher = commitDispatcher;
			this.commitFactory = commitFactory;
		}

		public IRepository OpenSession()
		{
			return new Repository(
				this.contextManager,
				this.store.OpenSession(),
				this.commitDispatcher,
				this.commitFactory );
		}
	}
}
