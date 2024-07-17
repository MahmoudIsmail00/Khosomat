using Khosomat.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khosomat.Entities.Interfaces
{
    public interface IProductReository : IGenericRepository<Product>
    {
        void Update(Product product);
    }
}
