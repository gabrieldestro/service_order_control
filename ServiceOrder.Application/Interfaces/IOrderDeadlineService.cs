using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Services.Interfaces
{
    public interface IOrderDeadlineService
    {
        bool AllDeadlinesRegistered(OrderDeadline d);
        Task<List<OrderDeadline>> GetAllAsync();
        Task<OrderDeadline?> GetByIdAsync(int id);
        Task<bool> AddAsync(OrderDeadline orderDeadline);
        Task<bool> UpdateAsync(OrderDeadline orderDeadline);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasDeadline(string orderName);
    }
}
