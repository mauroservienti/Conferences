using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Data
{
    public interface IRepositoryFactory
    {
        IRepository OpenSession();
    }
}
