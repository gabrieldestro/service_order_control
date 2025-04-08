using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrder.Domain.Entities;
using ServiceOrder.DTOs;
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
        public ObservableCollection<OrderDTO> Orders { get; set; }

        public OrderListView(
            IOrderService orderService,
            IOrderDeadlineService orderDeadlineService)
        {
            InitializeComponent();

            _orderService = orderService;
            _orderDeadlineService = orderDeadlineService;
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

                var deadlines = await Task.Run(() => _orderDeadlineService.GetAllAsync());
                var generalDeadline = deadlines.FirstOrDefault(d => String.IsNullOrEmpty(d.OrderId));
                
                foreach (var order in filteredOrders)
                {
                    var specificDeadline = deadlines.FirstOrDefault(d => d.OrderId == order.OrderName.ToString());
                    var deadline = specificDeadline ?? generalDeadline;

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
