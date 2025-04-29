using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ControlzEx.Theming;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Utils;
using ServiceOrder.Services.Interfaces;
using ServiceOrder.Utils;

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
                ValidateDates();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao carregar dados na tela de ordem.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao carregar dados da ordem de serviço.");
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
                DialogUtils.ShowInfo("Erro", "Erro ao carregar lista de clientes.");
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
                DialogUtils.ShowInfo("Erro", "Erro ao carregar companhias elétricas.");
            }
        }

        public void SetOrder(Order order)
        {

            if (order == null)
            {
                var new_order = new Order
                {
                    ReceivedDate = DateTime.Now,
                    LastUpdated = DateTime.Now
                };

                order = new_order;
            }
            else
            {
                ServiceOrderName.IsEnabled = false;

                ServiceOrderName.Text = order.OrderName;

                DescriptionTxt.Text = order.Description;
                ProjectValueTxt.Text = order.ProjectValue.ToString();

                _1_ReceiptDtPicker.SelectedDate = order.ReceivedDate;
                _2_DocSentDtPicker.SelectedDate = order.DocumentSentDate;
                _3_DocRecivedDtPicker.SelectedDate = order.DocumentReceivedDate;
                _4_ProjRegisteredDtPicker.SelectedDate = order.ProjectRegistrationDate;
                _5_ProjectSentDtPicker.SelectedDate = order.ProjectSubmissionDate;
                _6_ProjApprovedDtPicker.SelectedDate = order.ProjectApprovalDate;
                _7_RequestInspDtPicker.SelectedDate = order.InspectionRequestDate;
                _8_FinalizationDtPicker.SelectedDate = order.FinalizationDate;
                _9_PaymentDtPicker.SelectedDate = order.PaymentDate;
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
                DialogUtils.ShowInfo("Erro", "Erro ao abrir a tela de cliente.");
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
                DialogUtils.ShowInfo("Erro", "Erro ao abrir a tela de cliente final.");
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
                DialogUtils.ShowInfo("Erro", "Erro ao abrir a tela de companhia elétrica.");
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
                    DialogUtils.ShowInfo("Erro", "Erro ao salvar a ordem.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(currentOrder.OrderName))
                {
                    DialogUtils.ShowInfo("Erro", "Insira um código para a ordem de serviço!");
                    return;
                }

                if (_9_PaymentDtPicker.SelectedDate != null && String.IsNullOrEmpty(ProjectValueTxt.Text))
                {
                    DialogUtils.ShowInfo("Erro", "O projeto pago precisa ter valor definido!");
                    return;
                }

                if (ClientComboBox.SelectedValue is int clientId && clientId != 0)
                    currentOrder.ClientId = clientId;
                else
                {
                    DialogUtils.ShowInfo("Erro", "Selecione um cliente!");
                    return;
                }

                if (ClientFinalComboBox.SelectedValue is int finalClientId && finalClientId != 0)
                    currentOrder.FinalClientId = finalClientId;
                else
                {
                    DialogUtils.ShowInfo("Erro", "Selecione um cliente final!");
                    return;
                }

                if (ElectricCompanyComboBox.SelectedValue is int companyId && companyId != 0)
                    currentOrder.ElectricCompanyId = companyId;
                else
                {
                    DialogUtils.ShowInfo("Erro", "Selecione uma companhia elétrica!");
                    return;
                }

                currentOrder.LastUpdated = DateTime.Now;
                currentOrder.CreatedDate = DateTime.Now;

                currentOrder.Description = DescriptionTxt.Text;
                currentOrder.ProjectValue = ProjectValueTxt.Text.ToDecimalFromCurrencyOrNull();

                currentOrder.ReceivedDate = _1_ReceiptDtPicker.SelectedDate;
                currentOrder.DocumentSentDate = _2_DocSentDtPicker.SelectedDate;
                currentOrder.DocumentReceivedDate = _3_DocRecivedDtPicker.SelectedDate;
                currentOrder.ProjectRegistrationDate = _4_ProjRegisteredDtPicker.SelectedDate;
                currentOrder.ProjectSubmissionDate = _5_ProjectSentDtPicker.SelectedDate;
                currentOrder.ProjectApprovalDate = _6_ProjApprovedDtPicker.SelectedDate;
                currentOrder.InspectionRequestDate = _7_RequestInspDtPicker.SelectedDate;
                currentOrder.FinalizationDate = _8_FinalizationDtPicker.SelectedDate;
                currentOrder.PaymentDate = _9_PaymentDtPicker.SelectedDate;

                var success = false;
                if (currentOrder.Id == 0)
                    success = await _orderService.AddOrder(currentOrder);
                else
                    success = await _orderService.UpdateOrder(currentOrder);

                if (success)
                    DialogUtils.ShowInfo("Sucesso", "Projeto salvo com sucesso!");
                else
                    DialogUtils.ShowInfo("Erro", "Erro ao salvar projeto.");
                Close();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao salvar o projeto.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao salvar o projeto.");
            }

        }

        private void MonetaryTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "[0-9]");
        }

        private void MonetaryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            // Captura a posição atual do cursor
            int caret = textBox.CaretIndex;

            // Usa o Dispatcher para evitar loop de eventos
            textBox.Dispatcher.InvokeAsync(() =>
            {
                string digits = Regex.Replace(textBox.Text, @"[^\d]", "");

                if (decimal.TryParse(digits, out decimal number))
                {
                    number /= 100; // Ajusta para centavos
                    string formatted = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", number);

                    if (textBox.Text != formatted)
                    {
                        textBox.Text = formatted;
                        textBox.CaretIndex = formatted.Length;
                    }
                }
            }, DispatcherPriority.Background);
        }
    }
}
