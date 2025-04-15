using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;
using log4net.Config;
using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrder.IoC;
using ServiceOrder.Repository.Context;

namespace ServiceOrder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(App));

        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            DependencyInjection.ConfigureServices(services);

            // Registro das Views
            services.AddSingleton<MainView>();

            services.AddTransient<OptionsListView>();
            services.AddTransient<OrderListView>();
            services.AddTransient<OrderDeadlineListView>();
            services.AddTransient<ClientListView>();
            services.AddTransient<ElectricCompanyListView>();

            services.AddTransient<OrderDetailView>();
            services.AddTransient<OrderDeadlineDetailView>();
            services.AddTransient<ClientDetailView>();
            services.AddTransient<ElectricCompanyDetailView>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            _log.Info("Aplicação iniciada.");

            // Aplica as migrações para garantir que o banco de dados seja criado
            var dbContext = ServiceProvider.GetRequiredService<AppDbContext>();
            try
            {
                dbContext.Database.Migrate(); // Aplica as migrações pendentes
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao aplicar migrações: {ex.Message}.");
            }

            var mainWindow = ServiceProvider.GetRequiredService<MainView>();
            mainWindow.Show();
        }
    }
}
