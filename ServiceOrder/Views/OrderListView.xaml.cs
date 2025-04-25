using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using ServiceOrder.Domain.DTOs;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Services.Interfaces;
using ServiceOrder.Utils;

namespace ServiceOrder
{
    /// <summary>
    /// Interação lógica para OrderListView.xam
    /// </summary>
    public partial class OrderListView : UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(OrderListView));

        private readonly IOrderService _orderService;
        private readonly IOrderDeadlineService _orderDeadlineService;
        private readonly IClientService _clientService;
        private readonly IElectricCompanyService _electricCompanyService;
        private readonly ISpreadsheetService _spreadsheetService;
        public ObservableCollection<OrderDTO> Orders { get; set; }

        public OrderListView(
            IOrderService orderService,
            IOrderDeadlineService orderDeadlineService,
            IClientService clientService,
            IElectricCompanyService electricCompanyService,
            ISpreadsheetService spreadsheetService)
        {
            InitializeComponent();

            _orderService = orderService;
            _orderDeadlineService = orderDeadlineService;
            _clientService = clientService;
            _electricCompanyService = electricCompanyService;
            _spreadsheetService = spreadsheetService;

            Orders = new ObservableCollection<OrderDTO>();
            LoadOrdersWithFiltersAsync();
        }

        private void OnNewRegistrationClick(object sender, RoutedEventArgs e)
        {
            var detailView = App.ServiceProvider.GetRequiredService<OrderDetailView>();
            detailView.SetOrder(null); // Nova ordem
            detailView.Closed += (s, args) => LoadOrdersWithFiltersAsync();
            detailView.ShowDialog();
        }

        private async void OnExportClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Salvar como Excel",
                FileName = $"Projetos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            try
            {
                ChangeViewOnLoad(false);

                var memoryStream = _spreadsheetService.ExportOrdersToExcel(Orders.ToList());
                using (var fileStream = File.Create(saveFileDialog.FileName))
                {
                    await memoryStream.CopyToAsync(fileStream);
                }

            }
            catch (Exception ex)
            {
                _log.Error("Erro ao exportar projetos.", ex);
                DialogUtils.ShowInfo("Erro", $"Erro ao exportar projetos.");
                return;
            }
            finally
            {
                ChangeViewOnLoad(true);
            }

            DialogUtils.ShowInfo("Sucesso", "Arquivo salvo com sucesso!");
        }

        private void OnNextClick(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.Items.Count == 0)
            {
                DialogUtils.ShowInfo("Erro", "Nenhum registro disponível para edição.");
                return;
            }

            var items = OrderDataGrid.Items.Cast<OrderDTO>();
            var oldestItem = items.OrderByDescending(o => o.Order.LastUpdated).FirstOrDefault();

            if (oldestItem != null)
            {
                var detailView = App.ServiceProvider.GetRequiredService<OrderDetailView>();
                detailView.SetOrder(oldestItem.Order);
                detailView.Closed += (s, args) => LoadOrdersWithFiltersAsync();
                detailView.ShowDialog();
            }
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is OrderDTO selectedOrder)
            {
                var detailView = App.ServiceProvider.GetRequiredService<OrderDetailView>();
                detailView.SetOrder(selectedOrder.Order);
                detailView.Closed += (s, args) => LoadOrdersWithFiltersAsync();
                detailView.ShowDialog();
            }
        }

        private async void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is OrderDTO selectedOrder)
            {
                var result = DialogUtils.ShowConfirmation("Confirmação", $"Deseja excluir o projeto {selectedOrder?.Order?.Id}?");
                if (result)
                {
                    var success = await _orderService.DeleteOrder(selectedOrder?.Order);
                    if (!success)
                    {
                        DialogUtils.ShowInfo("Erro", "Erro ao excluir o projeto.");
                    }

                    LoadOrdersWithFiltersAsync();
                }
            }
        }

        private async void LoadOrdersAsync(Func<Order, bool> filter = null)
        {
            try
            {
                Orders.Clear();
                ChangeViewOnLoad(false);

                var startDate = DateTime.Today.AddDays(-90);
                var endDate = DateTime.Today.AddDays(90);
                List<OrderDTO> ordersFiltered = new List<OrderDTO>();

                if (StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate != null)
                {
                    startDate = StartDatePicker.SelectedDate.Value;
                    endDate = EndDatePicker.SelectedDate.Value;
                }

                var orders = await Task.Run(() => _orderService.GetOrdersAsync(startDate, endDate));
                var clients = await Task.Run(() => _clientService.GetAllAsync());
                var electricCompanies = await Task.Run(() => _electricCompanyService.GetAllAsync());

                foreach (var order in orders)
                {
                    order.Client = clients.FirstOrDefault(c => c.Id == order.ClientId);
                    order.ClientId = order.Client?.Id ?? 0;

                    order.FinalClient = clients.FirstOrDefault(c => c.Id == order.FinalClientId);
                    order.FinalClientId = order.FinalClient?.Id ?? 0;

                    order.ElectricCompany = electricCompanies.FirstOrDefault(c => c.Id == order.ElectricCompanyId);
                    order.ElectricCompanyId = order.ElectricCompany?.Id ?? 0;
                }

                var deadlines = await Task.Run(() => _orderDeadlineService.GetAllAsync());
                var generalDeadline = deadlines.FirstOrDefault(d => String.IsNullOrEmpty(d.OrderIdentifier));
                
                var filteredOrders = filter != null ? orders.Where(filter).ToList() : orders;

                foreach (var order in filteredOrders)
                {
                    var specificDeadline = deadlines.FirstOrDefault(d => d.OrderIdentifier == order.OrderName.ToString());
                    var deadline = specificDeadline ?? generalDeadline;

                    var dto = new OrderDTO
                    {
                        Order = order,
                        Deadline = deadline
                    };
                    ordersFiltered.Add(dto);
                }

                ordersFiltered = 
                    ordersFiltered
                    .Where(order => (ExpiredCheckBox.IsChecked == false) || order.IsAnyExpired())
                    .ToList();

                foreach (var order in ordersFiltered)
                    Orders.Add(order);

                OrderDataGrid.ItemsSource = Orders;
                UpdateRecordCount(Orders.Count);
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao exportar projetos.", ex);
                DialogUtils.ShowInfo("Erro", $"Erro ao carregar projetos.");
            }
            finally
            {
                ChangeViewOnLoad(true);
            }
        }

        private void ChangeViewOnLoad(bool show)
        {
            FilterButton.IsEnabled = show;
            ClearButton.IsEnabled = show;
            NewRegistrationButton.IsEnabled = show;
            NextButton.IsEnabled = show;
            ExportButton.IsEnabled = show;
            StartDatePicker.IsEnabled = show;
            EndDatePicker.IsEnabled = show;

            LoadingProgressBar.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }

        private void OnFilterClick(object sender, RoutedEventArgs e)
        {
            LoadOrdersWithFiltersAsync();
        }

        private void LoadOrdersWithFiltersAsync()
        {
            string searchText = SearchTextBox.Text.ToLower();

            LoadOrdersAsync(order =>
                (string.IsNullOrEmpty(searchText) 
                || order.OrderName.ToLower().Contains(searchText)
                || order.FinalClient?.Name?.ToLower().Contains(searchText) == true
                || order.Client?.Name?.ToLower().Contains(searchText) == true)
                && (PayedCheckBox.IsChecked == false || order.PaymentDate == null));
        }

        private void OnClearFiltersClick(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Clear();
            PayedCheckBox.IsChecked = false;
            ExpiredCheckBox.IsChecked = false;
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;

            LoadOrdersWithFiltersAsync();
        }

        private void UpdateRecordCount(int count)
        {
            RecordCountLabel.Content = count == 1
                ? $"{count} registro exibido."
                : $"{count} registros exibidos.";
        }
    }
}
