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
    public class ClientService : IClientService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ClientService));
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> AddAsync(Client client)
        {
            try
            {
                await _repository.AddAsync(client);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"Erro ao adicionar cliente '{client?.Name}': {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Client client)
        {
            try
            {
                await _repository.UpdateAsync(client);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"Erro ao atualizar cliente ID {client?.Id}: {ex.Message}", ex);
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
                _log.Error($"Erro ao remover cliente ID {id}: {ex.Message}", ex);
                return false;
            }
        }
    }
}
