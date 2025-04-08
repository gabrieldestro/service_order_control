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
    public class ElectricCompanyService : IElectricCompanyService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ElectricCompanyService));
        private readonly IElectricCompanyRepository _repository;

        public ElectricCompanyService(IElectricCompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ElectricCompany>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ElectricCompany?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> AddAsync(ElectricCompany company)
        {
            try
            {
                await _repository.AddAsync(company);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"Erro ao adicionar companhia elétrica '{company?.Name}': {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(ElectricCompany company)
        {
            try
            {
                await _repository.UpdateAsync(company);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"Erro ao atualizar companhia elétrica ID {company?.Id}: {ex.Message}", ex);
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
                _log.Error($"Erro ao remover companhia elétrica ID {id}: {ex.Message}", ex);
                return false;
            }
        }
    }
}
