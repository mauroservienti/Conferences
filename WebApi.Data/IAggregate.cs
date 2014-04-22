using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Data
{
	public interface IAggregate
	{
		String Id { get; }
		int Version { get; }

		Boolean IsChanged { get; }
		IEnumerable<IDomainEvent> GetUncommittedEvents();
		void ClearUncommittedEvents();
	}
}
