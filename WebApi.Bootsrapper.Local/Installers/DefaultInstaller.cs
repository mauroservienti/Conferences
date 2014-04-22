using Castle.MicroKernel.Registration;
using System.ComponentModel.Composition;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;

namespace WebApi.Bootsrapper.Local
{
	[Export( typeof( IWindsorInstaller ) )]
	public sealed class DefaultInstaller : IWindsorInstaller
	{
		public void Install( IWindsorContainer container, IConfigurationStore store )
		{
			var db = "FullStackSample";
			container.Register
			(
				Component.For<IDocumentStore>()
					.UsingFactoryMethod( ()=>
					{
						var ds = new DocumentStore()
						{
							Url = "http://localhost:8080/",
							DefaultDatabase = db
						}.Initialize();

						ds.DatabaseCommands.ForSystemDatabase().EnsureDatabaseExists( db );

						return ds;
					})
			);
		}
	}
}
