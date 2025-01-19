using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Services.Interfaces;

namespace ServiceOrder
{
    /// <summary>
    /// Interação lógica para OrderListView.xam
    /// </summary>
    public partial class OrderListView : Window
    {
        private readonly IOrderService _orderService;
        public ObservableCollection<Order> Orders { get; set; }

        public OrderListView(IOrderService orderService)
        {
            InitializeComponent();
            _orderService = orderService;
            Orders = new ObservableCollection<Order>();
            LoadOrders();
        }

        private void OnNovoCadastroClick(object sender, RoutedEventArgs e)
        {
            var detailView = App.ServiceProvider.GetRequiredService<OrderDetailView>();
            detailView.SetOrder(null); // Nova ordem (criação)
            detailView.Closed += (s, args) => LoadOrders();
            detailView.ShowDialog();
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Order selectedOrder)
            {
                var detailView = App.ServiceProvider.GetRequiredService<OrderDetailView>();
                detailView.SetOrder(selectedOrder); // Ordem existente (edição)
                detailView.Closed += (s, args) => LoadOrders();
                detailView.ShowDialog();
            }
        }

        private async void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Order selectedOrder)
            {
                var result = MessageBox.Show($"Deseja excluir a ordem {selectedOrder.Id}?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await _orderService.DeleteOrder(selectedOrder);
                    LoadOrders();
                }
            }
        }

        private async void LoadOrders(Func<Order, bool> filter = null)
        {
            Orders.Clear();

            var orders = await _orderService.GetAllAsync();
            foreach (var order in filter != null ? orders.Where(filter) : orders)
            {
                Orders.Add(order);
            }
            OrderDataGrid.ItemsSource = Orders;
        }
        
        private void OnFilterClick(object sender, RoutedEventArgs e)
        {
            // Obter valores dos filtros
            string searchText = SearchTextBox.Text.ToLower();
            string searchIdText = SearchIdTextBox.Text;
            string status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            LoadOrders(order =>
                (string.IsNullOrEmpty(searchText) || order.Name.ToLower().Contains(searchText)) &&
                (string.IsNullOrEmpty(searchIdText) || order.Id.ToString() == searchIdText) &&
                (string.IsNullOrEmpty(status) || order.status.ToString() == status) &&
                (!startDate.HasValue || order.CreateDate >= startDate.Value) &&
                (!endDate.HasValue || order.CreateDate <= endDate.Value));
        }

        private void OnClearFiltersClick(object sender, RoutedEventArgs e)
        {
            // Limpar campos de filtro
            SearchTextBox.Clear();
            SearchIdTextBox.Clear();
            StatusComboBox.SelectedIndex = 0;
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;

            // Recarregar todos os dados
            LoadOrders();
        }
    }
}
