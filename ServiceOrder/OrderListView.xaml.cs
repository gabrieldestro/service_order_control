using System.Collections.ObjectModel;
using System.Windows;
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

        public OrderListView()
        {
            InitializeComponent();
        }

        // Construtor com injeção de dependência
        public OrderListView(IOrderService orderService) : this()
        {
            _orderService = orderService;
            Orders = new ObservableCollection<Order>();
            LoadOrders();
        }

        private async void LoadOrders()
        {
            Orders.Clear();

            var orders = await _orderService.GetAllAsync();
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
            OrderDataGrid.ItemsSource = Orders;
        }

        private void OnNovoCadastroClick(object sender, RoutedEventArgs e)
        {
            var detailView = App.ServiceProvider.GetRequiredService<OrderDetailView>(); // Resolvido via DI
            detailView.Closed += (s, args) => LoadOrders(); // Recarregar ordens quando a janela for fechada
            detailView.ShowDialog();
        }
    }
}
