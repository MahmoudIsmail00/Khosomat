using Khosomat.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khosomat.Entities.Interfaces
{
    public interface IShoppingCartRepository:IGenericRepository<ShoppingCart>
    {
        int IncreaseCount(ShoppingCart shoppingCart, int Count);
        int DecreaseCount(ShoppingCart shoppingCart, int Count);
    }
}
