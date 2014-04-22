using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Indexes;
using Topics.Radical.Bootstrapper;

namespace WebApi.Data.Services
{
	class RavenDBIndexCreator : IRequireToStart
	{
		IDocumentStore store;

		public RavenDBIndexCreator( IDocumentStore store )
		{
			this.store = store;
		}

		public void Start()
		{
			IndexCreation.CreateIndexes( Assembly.GetExecutingAssembly(), this.store );
		}
	}
}
