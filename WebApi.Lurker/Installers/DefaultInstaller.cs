using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Topics.Radical.Reflection;
using WebApi.Data.Runtime;
using WebApi.Infrastructure;

namespace WebApi.Lurker.Installers
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
			container.Register
			(
				Component.For<IOperationContext>()
					.ImplementedBy<OperationContext>()
					.LifeStyle.Scoped()
			);
		}
	}
}