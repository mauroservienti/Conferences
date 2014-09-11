using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSB05Customer.Messages.Events
{
    public interface IShoppingCartCheckedout
    {
		String CartId { get; set; }
    }
}
