using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Services.Interfaces
{
    public interface IClientService
    {
        Task<List<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
        Task<bool> AddAsync(Client client);
        Task<bool> UpdateAsync(Client client);
        Task<bool> DeleteAsync(int id);
    }
}
