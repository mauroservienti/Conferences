using System.ComponentModel.Composition;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;
using Topics.Radical.Bootstrapper;
using Topics.Radical.Reflection;
using WebApi.Data.Runtime;
using WebApi.Infrastructure;

namespace WebApiServer.Installers
{
	/// <summary>
	/// Default boot installer.
	/// </summary>
	[Export( typeof( IWindsorInstaller ) )]
	public sealed class DefaultInstaller : IWindsorInstaller
	{
		/// <summary>
		/// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="store">The configuration store.</param>
		public void Install( IWindsorContainer container, IConfigurationStore store )
		{
			var boot = container.Resolve<IBootstrapper>();
			var directory = boot.ProbeDirectory;
			var filter = boot.AssemblyFilter;

			container.Register
			(
				Component.For<IOperationContext>()
					.ImplementedBy<OperationContext>()
					.LifeStyle.HybridPerWebRequestPerThread()
			);

			container.Register
			(
				Types.FromAssemblyInDirectory( new AssemblyFilter( directory, filter ) )
					.IncludeNonPublicTypes()
					.Where( t => !t.IsInterface && !t.IsAbstract && t.Is<Hub>() )
					.WithService.Self()
					.LifestyleTransient()
			);

			container.Register
			(
				Types.FromAssemblyInDirectory( new AssemblyFilter( directory, filter ) )
								.BasedOn<IHttpController>()
								.LifestyleTransient()
			);

			container.Register
			(
				Types.FromAssemblyInDirectory( new AssemblyFilter( directory, filter ) )
					.IncludeNonPublicTypes()
					.Where( t => t.Namespace != null && !t.IsAbstract && !t.IsInterface && t.Is<IFilter>() )
					.WithService.AllInterfaces()
					.LifestyleTransient()
			);
		}
	}
}