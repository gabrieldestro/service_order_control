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
using log4net;
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
        private static readonly ILog _log = LogManager.GetLogger(typeof(OrderDetailView));

        private readonly IOrderService _orderService;
        private readonly IClientService _clientService;
        private readonly IElectricCompanyService _electricCompanyService;

        private Order _order = new();
        private List<Client> _clients = new();
        private List<ElectricCompany> _electricCompanies = new();

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
            try
            {
                await LoadClientsAsync();
                await LoadElectricCompaniesAsync();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao carregar dados na tela de ordem.", ex);
                MessageBox.Show("Erro ao carregar dados da ordem de serviço.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadClientsAsync()
        {
            try
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
                    ClientFinalComboBox.SelectedValue = _order.FinalClientId;
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao carregar clientes.", ex);
                MessageBox.Show("Erro ao carregar lista de clientes.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadElectricCompaniesAsync()
        {
            try
            {
                _electricCompanies = (await _electricCompanyService.GetAllAsync()).OrderBy(c => c.Name).ToList();

                ElectricCompanyComboBox.ItemsSource = _electricCompanies;
                ElectricCompanyComboBox.DisplayMemberPath = "Name";
                ElectricCompanyComboBox.SelectedValuePath = "Id";

                if (_order.ElectricCompanyId != 0)
                    ElectricCompanyComboBox.SelectedValue = _order.ElectricCompanyId;
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao carregar companhias elétricas.", ex);
                MessageBox.Show("Erro ao carregar companhias elétricas.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            _2_DocSentDtPicker.IsEnabled = _1_ReceiptDtPicker.SelectedDate != null;
            _2_DocSentDtPickerBtn.IsEnabled = _1_ReceiptDtPicker.SelectedDate != null;

            _3_DocRecivedDtPicker.IsEnabled = _2_DocSentDtPicker.SelectedDate != null;
            _3_DocRecivedDtPickerBtn.IsEnabled = _2_DocSentDtPicker.SelectedDate != null;

            _4_ProjRegisteredDtPicker.IsEnabled = _3_DocRecivedDtPicker.SelectedDate != null;
            _4_ProjRegisteredDtPickerBtn.IsEnabled = _3_DocRecivedDtPicker.SelectedDate != null;

            _5_ProjectSentDtPicker.IsEnabled = _4_ProjRegisteredDtPicker.SelectedDate != null;
            _5_ProjectSentDtPickerBtn.IsEnabled = _4_ProjRegisteredDtPicker.SelectedDate != null;

            _6_ProjApprovedDtPicker.IsEnabled = _5_ProjectSentDtPicker.SelectedDate != null;
            _6_ProjApprovedDtPickerBtn.IsEnabled = _5_ProjectSentDtPicker.SelectedDate != null;

            _7_RequestInspDtPicker.IsEnabled = _6_ProjApprovedDtPicker.SelectedDate != null;
            _7_RequestInspDtPickerBtn.IsEnabled = _6_ProjApprovedDtPicker.SelectedDate != null;

            _8_FinalizationDtPicker.IsEnabled = _7_RequestInspDtPicker.SelectedDate != null;
            _8_FinalizationDtPickerBtn.IsEnabled = _7_RequestInspDtPicker.SelectedDate != null;
        }
        private void SetToday_Receipt(object sender, RoutedEventArgs e) =>
            _1_ReceiptDtPicker.SelectedDate = DateTime.Today;

        private void SetToday_DocSent(object sender, RoutedEventArgs e) =>
            _2_DocSentDtPicker.SelectedDate = DateTime.Today;

        private void SetToday_DocReceived(object sender, RoutedEventArgs e) =>
            _3_DocRecivedDtPicker.SelectedDate = DateTime.Today;

        private void SetToday_ProjRegistered(object sender, RoutedEventArgs e) =>
            _4_ProjRegisteredDtPicker.SelectedDate = DateTime.Today;

        private void SetToday_ProjectSent(object sender, RoutedEventArgs e) =>
            _5_ProjectSentDtPicker.SelectedDate = DateTime.Today;

        private void SetToday_ProjApproved(object sender, RoutedEventArgs e) =>
            _6_ProjApprovedDtPicker.SelectedDate = DateTime.Today;

        private void SetToday_RequestInsp(object sender, RoutedEventArgs e) =>
            _7_RequestInspDtPicker.SelectedDate = DateTime.Today;

        private void SetToday_Finalization(object sender, RoutedEventArgs e) =>
            _8_FinalizationDtPicker.SelectedDate = DateTime.Today;

        private void SetToday_Payment(object sender, RoutedEventArgs e) =>
            _9_PaymentDtPicker.SelectedDate = DateTime.Today;

        private void OnNewClientClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var detailView = App.ServiceProvider.GetRequiredService<ClientDetailView>();
                detailView.Closed += async (_, _) => await LoadClientsAsync();
                detailView.ShowDialog();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao abrir tela de novo cliente.", ex);
                MessageBox.Show("Erro ao abrir a tela de cliente.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnNewFinalClientClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var detailView = App.ServiceProvider.GetRequiredService<ClientDetailView>();
                detailView.Closed += async (_, _) => await LoadClientsAsync();
                detailView.ShowDialog();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao abrir tela de cliente final.", ex);
                MessageBox.Show("Erro ao abrir a tela de cliente final.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnNewElectricCompanyClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var detailView = App.ServiceProvider.GetRequiredService<ElectricCompanyDetailView>();
                detailView.Closed += async (_, _) => await LoadElectricCompaniesAsync();
                detailView.ShowDialog();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao abrir tela de companhia elétrica.", ex);
                MessageBox.Show("Erro ao abrir a tela de companhia elétrica.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is not Order currentOrder)
                {
                    MessageBox.Show("Erro ao salvar a ordem.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(currentOrder.OrderName))
                {
                    MessageBox.Show("Insira um código para a ordem de serviço!", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (ClientComboBox.SelectedValue is int clientId && clientId != 0)
                    currentOrder.ClientId = clientId;
                else
                {
                    MessageBox.Show("Selecione um cliente!", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (ClientFinalComboBox.SelectedValue is int finalClientId && finalClientId != 0)
                    currentOrder.FinalClientId = finalClientId;
                else
                {
                    MessageBox.Show("Selecione um cliente final!", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (ElectricCompanyComboBox.SelectedValue is int companyId && companyId != 0)
                    currentOrder.ElectricCompanyId = companyId;
                else
                {
                    MessageBox.Show("Selecione uma companhia elétrica!", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            catch (Exception ex)
            {
                _log.Error("Erro ao salvar a ordem de serviço.", ex);
                MessageBox.Show("Erro ao salvar a ordem de serviço.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
