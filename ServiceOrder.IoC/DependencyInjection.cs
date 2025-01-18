using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Repository.Context;
using ServiceOrder.Repository.Repositories;
using ServiceOrder.Services.Interfaces;
using ServiceOrder.Services.Services;

namespace ServiceOrder.IoC
{
    public static class DependencyInjection
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Configuração do SQLite e DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=serviceorders.db"));

            // Registro do repositório
            services.AddScoped<IOrderRepository, OrderRepository>();

             // Registro do Serviço
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}
