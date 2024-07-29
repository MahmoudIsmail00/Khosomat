using Khosomat.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khosomat.Entities.Interfaces
{
    public interface IOrderHeaderRepository:IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateOrderStatus(int id,string OrderStatus,string PaymentStatus);
    }
}
