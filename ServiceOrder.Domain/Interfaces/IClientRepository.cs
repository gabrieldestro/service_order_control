using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Domain.Interfaces
{
    public interface IClientRepository
    {
        Task<List<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
        Task AddAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(int id);
    }   
}
