using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Services.Interfaces;

namespace ServiceOrder.Services.Services
{
    public class OrderDeadlineService : IOrderDeadlineService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(OrderDeadlineService));
        private readonly IOrderDeadlineRepository _repository;

        public OrderDeadlineService(IOrderDeadlineRepository repository)
        {
            _repository = repository;
        }

        public bool AllDeadlinesRegistered(OrderDeadline d)
        {
            return d.DocumentSentDays != null
                && d.DocumentReceivedDays != null
                && d.ProjectRegistrationDays != null
                && d.ProjectSubmissionDays != null
                && d.ProjectApprovalDays != null
                && d.InspectionRequestDays != null
                && d.FinalizationDays != null
                && d.PaymentDays != null;
        }

        public async Task<List<OrderDeadline>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<OrderDeadline?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> HasDeadline(string orderName)
        {
            try
            {
                return await _repository.HasDeadlineForOrdername(orderName);
            }
            catch (Exception ex)
            {
                _log.Error($"Erro ao buscar prazo para ordem (OrderId={orderName}): {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> AddAsync(OrderDeadline orderDeadline)
        {
            try
            {
                await _repository.AddAsync(orderDeadline);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"Erro ao adicionar prazo da ordem (OrderId={orderDeadline?.OrderIdentifier}): {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(OrderDeadline orderDeadline)
        {
            try
            {
                await _repository.UpdateAsync(orderDeadline);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"Erro ao atualizar prazo da ordem ID {orderDeadline?.Id}: {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"Erro ao remover prazo da ordem ID {id}: {ex.Message}", ex);
                return false;
            }
        }
    }
}
