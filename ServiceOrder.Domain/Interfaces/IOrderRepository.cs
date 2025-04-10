using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Entities.Order>> GetOrdersAsync(DateTime startDate, DateTime endDate);
        Task<List<Entities.Order>> GetPendingOrdersAsync();
        Task<List<Entities.Order>> GetAllAsync();
        Task<Entities.Order> GetByIdAsync(int id);
        Task AddAsync(Entities.Order order);
        Task UpdateAsync(Entities.Order order);
        Task DeleteAsync(int id);
    }
}
