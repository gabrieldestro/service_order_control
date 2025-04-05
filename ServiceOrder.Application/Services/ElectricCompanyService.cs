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
    public class ElectricCompanyService : IElectricCompanyService
    {
        private readonly IElectricCompanyRepository _repository;

        public ElectricCompanyService(IElectricCompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ElectricCompany>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<ElectricCompany?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<bool> AddAsync(ElectricCompany company)
        {
            try
            {
                await _repository.AddAsync(company);
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateAsync(ElectricCompany company)
        {
            try
            {
                await _repository.UpdateAsync(company);
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
