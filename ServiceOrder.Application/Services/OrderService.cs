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

        public async Task<bool> AddOrder(Order order)
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

        public async Task<List<Order>> GetAllAsync()
        {
            List<Order> list = new List<Order>();

            try
            {
                list = await _orderRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
            }

            return list;
        }

        public async Task<bool> UpdateOrder(Order order)
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
