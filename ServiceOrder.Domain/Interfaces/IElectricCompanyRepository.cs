using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Domain.Interfaces
{
    public interface IElectricCompanyRepository
    {
        Task<List<ElectricCompany>> GetAllAsync();
        Task<ElectricCompany?> GetByIdAsync(int id);
        Task AddAsync(ElectricCompany company);
        Task UpdateAsync(ElectricCompany company);
        Task DeleteAsync(int id);
    }
}
