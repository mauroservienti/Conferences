using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.Data
{
	public interface IRepository : IDisposable
	{
		void Save<TAggregate>( TAggregate aggregate ) where TAggregate : IAggregate;
		void CommitChanges();

		TAggregate GetById<TAggregate>( String aggregateId ) where TAggregate : IAggregate;

		TAggregate[] GetById<TAggregate>( params string[] aggregateIds ) where TAggregate : IAggregate;
	}
}
