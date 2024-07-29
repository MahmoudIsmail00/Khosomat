using Khosomat.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khosomat.Entities.Interfaces
{
    public interface IOrderDetailRepository:IGenericRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    }
}
