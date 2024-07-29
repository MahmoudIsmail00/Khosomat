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
        IShoppingCartRepository ShoppingCart { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }
        IApplicationUserRepository ApplicationUser { get; }

		int Complete();
    }
}
