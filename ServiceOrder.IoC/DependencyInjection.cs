using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                options.UseSqlite("Data Source=serviceorders.db")
                .LogTo(message => Debug.WriteLine(message), LogLevel.Information)
                .EnableSensitiveDataLogging());

            // Registro do repositório
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDeadlineService, OrderDeadlineService>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IElectricCompanyRepository, ElectricCompanyRepository>();

             // Registro do Serviço
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDeadlineRepository, OrderDeadlineRepository>();
            services.AddScoped<IElectricCompanyService, ElectricCompanyService>();
            services.AddScoped<IClientService, ClientService>();

            services.AddScoped<ISpreadsheetService, SpreadsheetService>();
        }
    }
}
