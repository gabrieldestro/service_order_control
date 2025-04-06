using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Services.Interfaces;

namespace ServiceOrder.Services.Services
{
    public class OrderDeadlineService : IOrderDeadlineService
    {
        private readonly IOrderDeadlineRepository _repository;

        public OrderDeadlineService(IOrderDeadlineRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderDeadline>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<OrderDeadline?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<bool> AddAsync(OrderDeadline orderDeadline)
        {
            try
            {
                await _repository.AddAsync(orderDeadline);
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateAsync(OrderDeadline orderDeadline)
        {
            try
            {
                await _repository.UpdateAsync(orderDeadline);
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return true;
            }
            catch { return false; }
        }
    }
}
