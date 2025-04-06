using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Domain.Interfaces
{
    public interface IOrderDeadlineRepository
    {
        Task<List<OrderDeadline>> GetAllAsync();
        Task<OrderDeadline?> GetByIdAsync(int id);
        Task AddAsync(OrderDeadline orderDeadline);
        Task UpdateAsync(OrderDeadline orderDeadline);
        Task DeleteAsync(int id);
    }   
}
