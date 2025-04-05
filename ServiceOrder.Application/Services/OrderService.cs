using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Repository.Repositories;
using ServiceOrder.Services.Interfaces;

namespace ServiceOrder.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> AddOrder(Domain.Entities.Order order)
        {
            try
            {
                await _orderRepository.AddAsync(order);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteOrder(Domain.Entities.Order order)
        {
            try
            {
                await _orderRepository.DeleteAsync(order.Id);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<List<Domain.Entities.Order>> GetAllAsync()
        {
            List<Domain.Entities.Order> list = new List<Domain.Entities.Order>();

            try
            {
                list = await _orderRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
            }

            return list;
        }

        public async Task<bool> UpdateOrder(Domain.Entities.Order order)
        {
            try
            {
                await _orderRepository.UpdateAsync(order);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
