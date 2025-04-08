using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using ServiceOrder.Domain.DTOs;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Services.Interfaces;

namespace ServiceOrder
{
    /// <summary>
    /// Interação lógica para OrderListView.xam
    /// </summary>
    public partial class OrderListView : UserControl
    {
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
            LoadOrdersAsync();
        }

        private void OnNewRegistrationClick(object sender, RoutedEventArgs e)
        {
            var detailView = App.ServiceProvider.GetRequiredService<OrderDetailView>();
            detailView.SetOrder(null); // Nova ordem
            detailView.Closed += (s, args) => LoadOrdersAsync();
            detailView.ShowDialog();
        }

        private void OnExportClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Salvar como Excel",
                FileName = $"Projetos_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}.xlsx"
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            var memoryStream = _spreadsheetService.ExportOrdersToExcel(Orders.ToList());
            using (var fileStream = File.Create(saveFileDialog.FileName))
            {
                memoryStream.CopyTo(fileStream);
            }

            MessageBox.Show("Arquivo salvo com sucesso!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnNextClick(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.Items.Count == 0)
            {
                MessageBox.Show("Nenhum registro disponível para edição.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var items = OrderDataGrid.Items.Cast<OrderDTO>();
            var oldestItem = items.OrderByDescending(o => o.Order.LastUpdated).FirstOrDefault();

            if (oldestItem != null)
            {
                var detailView = App.ServiceProvider.GetRequiredService<OrderDetailView>();
                detailView.SetOrder(oldestItem.Order);
                detailView.Closed += (s, args) => LoadOrdersAsync();
                detailView.ShowDialog();
            }
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is OrderDTO selectedOrder)
            {
                var detailView = App.ServiceProvider.GetRequiredService<OrderDetailView>();
                detailView.SetOrder(selectedOrder.Order);
                detailView.Closed += (s, args) => LoadOrdersAsync();
                detailView.ShowDialog();
            }
        }

        private async void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is OrderDTO selectedOrder)
            {
                var result = MessageBox.Show($"Deseja excluir a ordem {selectedOrder?.Order?.Id}?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await _orderService.DeleteOrder(selectedOrder?.Order);
                    LoadOrdersAsync();
                }
            }
        }

        private async void LoadOrdersAsync(Func<Order, bool> filter = null)
        {
            try
            {
                Orders.Clear();
                ChangeViewOnLoad(false);

                await Task.Delay(1000);

                var orders = await Task.Run(() => _orderService.GetAllAsync());
                var filteredOrders = filter != null ? orders.Where(filter) : orders;

                var clients = await Task.Run(() => _clientService.GetAllAsync());
                var electricCompanies = await Task.Run(() => _electricCompanyService.GetAllAsync());

                var deadlines = await Task.Run(() => _orderDeadlineService.GetAllAsync());
                var generalDeadline = deadlines.FirstOrDefault(d => String.IsNullOrEmpty(d.OrderId));
                
                foreach (var order in filteredOrders)
                {
                    var specificDeadline = deadlines.FirstOrDefault(d => d.OrderId == order.OrderName.ToString());
                    var deadline = specificDeadline ?? generalDeadline;

                    order.Client = clients.FirstOrDefault(c => c.Id == order.ClientId);
                    order.FinalClient = clients.FirstOrDefault(c => c.Id == order.FinalClientId);
                    order.ElectricCompany = electricCompanies.FirstOrDefault(c => c.Id == order.ElectricCompanyId);

                    var dto = new OrderDTO
                    {
                        Order = order,
                        Deadline = deadline
                    };
                    Orders.Add(dto);
                }

                OrderDataGrid.ItemsSource = Orders;
                UpdateRecordCount(Orders.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar ordens: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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

            LoadingProgressBar.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }

        private void OnFilterClick(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            string searchIdText = SearchIdTextBox.Text;
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            LoadOrdersAsync(order =>
                (string.IsNullOrEmpty(searchIdText) || order.Id.ToString() == searchIdText) &&
                (string.IsNullOrEmpty(searchText) || order.Client?.Name?.ToLower().Contains(searchText) == true) &&
                (!startDate.HasValue || order.ReceivedDate >= startDate.Value) &&
                (!endDate.HasValue || order.ReceivedDate <= endDate.Value));
        }

        private void OnClearFiltersClick(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Clear();
            SearchIdTextBox.Clear();
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
            LoadOrdersAsync();
        }

        private void UpdateRecordCount(int count)
        {
            RecordCountLabel.Content = count == 1
                ? $"{count} registro exibido."
                : $"{count} registros exibidos.";
        }
    }
}
