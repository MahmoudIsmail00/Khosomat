using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khosomat.Entities.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryReository Category { get; }
        IProductReository Product { get; }

        int Complete();
    }
}
