using Castle.MicroKernel.Registration;
using System.ComponentModel.Composition;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;
using Topics.Radical.Reflection;
using System;
using System.Linq;
using Topics.Radical.ComponentModel;
using Raven.Client.Indexes;
using System.Reflection;
using CQRS.Views;

namespace CQRS.Installers
{
	[Export( typeof( IWindsorInstaller ) )]
	public sealed class DefaultInstaller : IWindsorInstaller
	{
		public void Install( IWindsorContainer container, IConfigurationStore store )
		{
			container.Register
			(
				Component.For<IDocumentSession>()
					.UsingFactoryMethod( () =>
					{
						var ds = container.Resolve<IDocumentStore>();
						return ds.OpenSession();
					} )
					.LifeStyle.Is( Castle.Core.LifestyleType.Scoped )
					.OnDestroy( ( k, s ) =>
					{
						s.SaveChanges();
						s.Dispose();
					} )
			);

			container.Register
			(
				Component.For<IDocumentStore>()
					.UsingFactoryMethod( () =>
					{
						var ds = new DocumentStore()
						{
							ConnectionStringName = "CQRS/Persistence"
						};

						ds.Initialize();

						IndexCreation.CreateIndexes( typeof( SearchResult ).Assembly, ds );
						IndexCreation.CreateIndexes( Assembly.GetExecutingAssembly(), ds );

						return ds;
					} )
					.LifestyleSingleton()
			);
		}
	}
}
