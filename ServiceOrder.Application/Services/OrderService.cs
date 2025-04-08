using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Repository.Repositories;
using ServiceOrder.Services.Interfaces;

namespace ServiceOrder.Services.Services
{
    public class OrderService : IOrderService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(OrderService));
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
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"Erro ao adicionar ordem '{order?.OrderName}' (ID={order?.Id}): {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> DeleteOrder(Domain.Entities.Order order)
        {
            try
            {
                await _orderRepository.DeleteAsync(order.Id);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"Erro ao deletar ordem ID {order?.Id}: {ex.Message}", ex);
                return false;
            }
        }

        public async Task<List<Domain.Entities.Order>> GetAllAsync()
        {
            try
            {
                return await _orderRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao obter todas as ordens: " + ex.Message, ex);
                return new List<Domain.Entities.Order>();
            }
        }

        public async Task<bool> UpdateOrder(Domain.Entities.Order order)
        {
            try
            {
                await _orderRepository.UpdateAsync(order);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"Erro ao atualizar ordem '{order?.OrderName}' (ID={order?.Id}): {ex.Message}", ex);
                return false;
            }
        }
    }
}
