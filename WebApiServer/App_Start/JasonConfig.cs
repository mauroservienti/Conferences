using System;
using System.IO;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Jason.Configuration;
using Jason.Factories;
using Jason.WebAPI.Validation;
using Newtonsoft.Json;
using Topics.Radical.Reflection;
using WebApi.Infrastructure;
using WebApiServer.Infrastructure;

namespace WebApiServer
{
	public class JasonConfig
	{
		public static void Initialize( String pathToScanForAssemblies, String assemblySelectPattern, IWindsorContainer container )
		{
			var jasonConfig = new DefaultJasonServerConfiguration
			(
				pathToScanForAssemblies: Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "bin" ),
				assemblySelectPattern: assemblySelectPattern
			)
			{
				Container = new WindsorJasonContainerProxy( container ),
				TypeFilter = t => !t.Is<JasonToBusRepublishHandler>()
			};

			jasonConfig.AddEndpoint( new Jason.WebAPI.JasonWebAPIEndpoint()
			{
				TypeNameHandling = TypeNameHandling.Objects,
				DefaultSuccessfulHttpResponseCode = System.Net.HttpStatusCode.Accepted,
				OnExecutingAction = ( e, request ) =>
				{
					if ( !e.RequestContainsCorrelationId )
					{
						e.CorrelationId = Guid.NewGuid().ToString();
						e.AppendCorrelationIdToResponse = true;
					}

					var operationContextManager = container.Resolve<IOperationContextManager>();
					var context = operationContextManager.GetCurrent();
					context.ForOperation( e.CorrelationId );
				}
			} )
			.UsingAsFallbackCommandHandler<JasonToBusRepublishHandler>()
			.UsingAsFallbackCommandValidator<ObjectDataAnnotationValidator>()
			.Initialize();
		}
	}
}