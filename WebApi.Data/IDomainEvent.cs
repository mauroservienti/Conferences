using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Data
{
	public interface IDomainEvent
	{
		String Id { get; }
		String AggregateId { get; }
		DateTimeOffset OccurredAt { get; }
	}
}
