using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Services.Interfaces
{
    public interface IElectricCompanyService
    {
        Task<List<ElectricCompany>> GetAllAsync();
        Task<ElectricCompany?> GetByIdAsync(int id);
        Task<bool> AddAsync(ElectricCompany company);
        Task<bool> UpdateAsync(ElectricCompany company);
        Task<bool> DeleteAsync(int id);
    }
}
