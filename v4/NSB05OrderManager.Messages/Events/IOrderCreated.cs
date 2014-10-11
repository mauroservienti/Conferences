using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB05OrderManager.Messages.Events
{
    public interface IOrderCreated
    {
		string OrderId { get; set; }
	}
}
