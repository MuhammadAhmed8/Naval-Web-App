using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public interface IOrderRepository
    {
        void SaveOrder(Order order);
        IEnumerable<Order> GetOrders();
        void MarkDelivered(int OrderId);
        void CancelOrder(int orderId);
    }
}
