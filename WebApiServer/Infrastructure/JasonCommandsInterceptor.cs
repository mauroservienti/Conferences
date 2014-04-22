using System;
using Jason.Handlers;
using Merp.Web.Infrastructure;
using Topics.Radical.Validation;
using Topics.Radical.Reflection;
using Raven.Client;
using WebApi.Data.Requests;
using System.Threading;
using WebApi.Infrastructure;

namespace Merp.Web.Infrastructure
{
	class JasonCommandsInterceptor : ICommandInterceptor
	{
		readonly IOperationContextManager operationContextManager;
		readonly IDocumentStore store;

		public JasonCommandsInterceptor( IOperationContextManager operationContextManager, IDocumentStore store )
		{
			Ensure.That( operationContextManager ).Named( () => operationContextManager ).IsNotNull();
			Ensure.That( store ).Named( () => store ).IsNotNull();

			this.operationContextManager = operationContextManager;
			this.store = store;
		}

		public void OnException( object rawCommand, Exception exception )
		{
			var context = this.operationContextManager.GetCurrent();

			using ( var session = this.store.OpenSession() )
			{
				var commandException = new CommandException
				(
					//id: "commandExceptions/" + Guid.NewGuid().ToString(),
					correlationId: context.CorrelationId,
					rawCommand: rawCommand,
					userAccount: Thread.CurrentPrincipal.Identity.Name,
					error: exception
				);

				session.Store( commandException );
				session.SaveChanges();
			}
		}

		public void OnExecute( object rawCommand )
		{
			var context = this.operationContextManager.GetCurrent();
			using ( var session = this.store.OpenSession() )
			{
				var commandRequest = new CommandRequest
				(
					correlationId: context.CorrelationId,
					rawCommand: rawCommand,
					userAccount: Thread.CurrentPrincipal.Identity.Name
				);

				session.Store( commandRequest );
				session.SaveChanges();
			}
		}

		public void OnExecuted( object rawCommand, object rawResult )
		{
			var context = this.operationContextManager.GetCurrent();
			using ( var session = this.store.OpenSession() )
			{
				var commandResponse = new CommandResponse
				(
					correlationId: context.CorrelationId,
					rawResponse: rawResult,
					userAccount: Thread.CurrentPrincipal.Identity.Name
				);

				session.Store( commandResponse );
				session.SaveChanges();
			}
		}
	}
}