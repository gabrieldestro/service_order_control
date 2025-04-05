using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceOrder
{
    /// <summary>
    /// Lógica interna para MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private readonly IServiceProvider _serviceProvider;

        public MainView(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            ShowOrders(null, null); // Página padrão
        }

        private void ShowOrders(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _serviceProvider.GetRequiredService<OrderListView>();
        }

        private void ShowClients(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _serviceProvider.GetRequiredService<ClientListView>();
        }

        private void ShowElectricCompanies(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _serviceProvider.GetRequiredService<ElectricCompanyListView>();
        }

        private void ShowOptions(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _serviceProvider.GetRequiredService<OptionsListView>();
        }
    }
}
