using Finanzauto.Domain.DTOS.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO?> GetOrderById(long id);
        Task<IEnumerable<OrderDTO>> GetOrdersPaged(int page, int pageSize);
        Task<OrderDTO?> CreateOrder(OrderCreateDTO dto);
        Task<bool> UpdateOrder(long id, OrderCreateDTO dto);
        Task<bool> DeleteOrder(long id);
    }
}
