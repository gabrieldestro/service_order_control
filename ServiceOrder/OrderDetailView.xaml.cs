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
using Microsoft.Extensions.DependencyInjection;
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

        private Order _order = new Order();
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
            await LoadClientsAsync();
            await LoadElectricCompaniesAsync();
        }

        private async Task LoadClientsAsync()
        {
            _clients = (await _clientService.GetAllAsync()).OrderBy(c => c.Name).ToList();

            ClientComboBox.ItemsSource = _clients;
            ClientComboBox.DisplayMemberPath = "Name";
            ClientComboBox.SelectedValuePath = "Id";

            ClientFinalComboBox.ItemsSource = _clients;
            ClientFinalComboBox.DisplayMemberPath = "Name";
            ClientFinalComboBox.SelectedValuePath = "Id";

            if (_order.ClientId != 0)
                ClientComboBox.SelectedValue = _order.ClientId;

            if (_order.FinalClientId != 0)
                ClientFinalComboBox.SelectedValue = _order.FinalClient;
        }

        private async Task LoadElectricCompaniesAsync()
        {
            _electricCompanies = (await _electricCompanyService.GetAllAsync()).OrderBy(c => c.Name).ToList();

            ElectricCompanyComboBox.ItemsSource = _electricCompanies;
            ElectricCompanyComboBox.DisplayMemberPath = "Name";
            ElectricCompanyComboBox.SelectedValuePath = "Id";


            if (_order.ElectricCompanyId != 0)
                ElectricCompanyComboBox.SelectedValue = _order.ElectricCompanyId;
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
            else
            {
                ServiceOrderName.IsEnabled = false;
            }

            _order = order;
            DataContext = order;
        }

        private void AnyDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateDates();
        }

        private void ValidateDates()
        {
            /*
            if (_8_FinalizationDtPicker.SelectedDate != null)
                _9_PaymentDtPicker.IsEnabled = true;
            else
                _9_PaymentDtPicker.IsEnabled = false;
            */
            if (_7_RequestInspDtPicker.SelectedDate != null)
                _8_FinalizationDtPicker.IsEnabled = true;
            else
                _8_FinalizationDtPicker.IsEnabled = false;

            if (_6_ProjApprovedDtPicker.SelectedDate != null)
                _7_RequestInspDtPicker.IsEnabled = true;
            else
                _7_RequestInspDtPicker.IsEnabled = false;

            if (_5_ProjectSentDtPicker.SelectedDate != null)
                _6_ProjApprovedDtPicker.IsEnabled = true;
            else
                _6_ProjApprovedDtPicker.IsEnabled = false;

            if (_4_ProjRegisteredDtPicker.SelectedDate != null)
                _5_ProjectSentDtPicker.IsEnabled = true;
            else
                _5_ProjectSentDtPicker.IsEnabled = false;

            if (_3_DocRecivedDtPicker.SelectedDate != null)
                _4_ProjRegisteredDtPicker.IsEnabled = true;
            else
                _4_ProjRegisteredDtPicker.IsEnabled = false;

            if (_2_DocSentDtPicker.SelectedDate != null)
                _3_DocRecivedDtPicker.IsEnabled = true;
            else
                _3_DocRecivedDtPicker.IsEnabled = false;

            if (_1_ReceiptDtPicker.SelectedDate != null)
                _2_DocSentDtPicker.IsEnabled = true;
            else
                _2_DocSentDtPicker.IsEnabled = false;
        }

        private void OnNewClientClick(object sender, RoutedEventArgs e)
        {
            var detailView = App.ServiceProvider.GetRequiredService<ClientDetailView>();
            detailView.Closed += async (s, args) => await LoadClientsAsync();
            detailView.ShowDialog();
        }

        private void OnNewFinalClientClick(object sender, RoutedEventArgs e)
        {
            var detailView = App.ServiceProvider.GetRequiredService<ClientDetailView>();
            detailView.Closed += async (s, args) => await LoadClientsAsync();
            detailView.ShowDialog();
        }

        private void OnNewElectricCompanyClick(object sender, RoutedEventArgs e)
        {
            var detailView = App.ServiceProvider.GetRequiredService<ElectricCompanyDetailView>();
            detailView.Closed += async (s, args) => await LoadElectricCompaniesAsync();
            detailView.ShowDialog();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not Order currentOrder)
            {
                MessageBox.Show("Erro ao salvar a ordem.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrEmpty(currentOrder.OrderName))
            {
                MessageBox.Show("Insira um código para a ordem de serviço!.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ClientComboBox.SelectedValue != null && (int)ClientComboBox.SelectedValue != 0)
                currentOrder.ClientId = (int)ClientComboBox.SelectedValue;
            else
            {
                MessageBox.Show("Selecione um cliente!.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ClientFinalComboBox.SelectedValue != null && (int)ClientFinalComboBox.SelectedValue != 0)
                currentOrder.ClientId = (int)ClientFinalComboBox.SelectedValue;
            else
            {
                MessageBox.Show("Selecione um cliente final!.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ElectricCompanyComboBox.SelectedValue != null && (int)ElectricCompanyComboBox.SelectedValue != 0)
                currentOrder.ElectricCompanyId = (int)ElectricCompanyComboBox.SelectedValue;
            else
            {
                MessageBox.Show("Selecione uma companhia elétrica!.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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
