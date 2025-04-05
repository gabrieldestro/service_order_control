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
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Client>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<Client?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<bool> AddAsync(Client client)
        {
            try
            {
                await _repository.AddAsync(client);
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateAsync(Client client)
        {
            try
            {
                await _repository.UpdateAsync(client);
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
