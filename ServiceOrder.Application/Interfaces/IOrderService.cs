using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Domain.Entities.Order>> GetOrdersAsync(DateTime startdate, DateTime EndDate);
        Task<List<Domain.Entities.Order>> GetPendingOrdersAsync();
        Task<List<Domain.Entities.Order>> GetAllAsync();
        Task<bool> AddOrder(Domain.Entities.Order order);
        Task<bool> UpdateOrder(Domain.Entities.Order order);
        Task<bool> DeleteOrder(Domain.Entities.Order order);
    }
}
