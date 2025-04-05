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
using ControlzEx.Theming;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Repository.Repositories;
using ServiceOrder.Services.Interfaces;

namespace ServiceOrder
{
    /// <summary>
    /// Lógica interna para OrderDetailView.xaml
    /// </summary>
    public partial class OrderDetailView : Window
    {
        private readonly IOrderService _orderService;
        private readonly IClientService _clientService;
        private readonly IElectricCompanyService _electricCompanyService;

        private List<Client> _clients = [];
        private List<ElectricCompany> _electricCompanies = [];

        public OrderDetailView(
            IOrderService orderService,
            IClientService clientService,
            IElectricCompanyService electricCompanyService)
        {
            InitializeComponent();
            _orderService = orderService;
            _clientService = clientService;
            _electricCompanyService = electricCompanyService;

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();

            Loaded += OrderDetailView_Loaded;
        }

        private async void OrderDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            _clients = (await _clientService.GetAllAsync()).ToList();
            _electricCompanies = (await _electricCompanyService.GetAllAsync()).ToList();

            ClientComboBox.ItemsSource = _clients;
            ClientComboBox.DisplayMemberPath = "Name";
            ClientComboBox.SelectedValuePath = "Id";

            ElectricCompanyComboBox.ItemsSource = _electricCompanies;
            ElectricCompanyComboBox.DisplayMemberPath = "Name";
            ElectricCompanyComboBox.SelectedValuePath = "Id";
        }

        public void SetOrder(Order order)
        {
            if (order == null)
            {
                order = new Order
                {
                    ReceivedDate = DateTime.Now,
                    LastUpdated = DateTime.Now
                };
            }

            DataContext = order;
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not Order currentOrder)
            {
                MessageBox.Show("Erro ao salvar a ordem.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ClientComboBox.SelectedValue is int clientId)
                currentOrder.ClientId = clientId;

            if (ElectricCompanyComboBox.SelectedValue is int companyId)
                currentOrder.ElectricCompanyId = companyId;

            currentOrder.LastUpdated = DateTime.Now;

            if (currentOrder.Id == 0)
                await _orderService.AddOrder(currentOrder);
            else
                await _orderService.UpdateOrder(currentOrder);

            MessageBox.Show("Ordem de serviço salva com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
