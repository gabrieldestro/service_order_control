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

        public Dictionary<Order.Status, string> statusWithCaptions { get; } =
            new Dictionary<Order.Status, string>()
            {
                {Order.Status.STARTED, "Inciado"},
                {Order.Status.PENDING, "Pendente"},
                {Order.Status.FINISHED, "Finalizado"}
            };


        private Order.Status _orderStatus;
        public Order.Status orderStatus
        {
            get { return _orderStatus; }
            set {; }
        }

        public OrderDetailView(IOrderService orderService)
        {
            InitializeComponent();
            _orderService = orderService;

            // Define o DataContext para expor a lista de status
            DataContext = this;
        }

        public void SetOrder(Order order)
        {
            if (order == null)
            {
                order = new Order
                {
                    CreateDate = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    status = Order.Status.STARTED
                };
            }

            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Tag.ToString() == order.status.ToString())
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }

            // Define o DataContext com a ordem atual ou nova
            DataContext = order;
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not Order currentOrder)
            {
                MessageBox.Show("Erro ao salvar a ordem. Tente novamente.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar os campos obrigatórios
            if (string.IsNullOrWhiteSpace(currentOrder.Name) || string.IsNullOrWhiteSpace(currentOrder.Description))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Atualizar a data de modificação
            currentOrder.LastUpdated = DateTime.Now;
            // Pega o status selecionado no ComboBox e faz o mapeamento
            if (StatusComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                currentOrder.status = selectedItem.Tag.ToString() switch
                {
                    "STARTED" => Order.Status.STARTED,
                    "PENDING" => Order.Status.PENDING,
                    "FINISHED" => Order.Status.FINISHED,
                    _ => Order.Status.STARTED, // Valor padrão
                };
            }

            // Salvar no serviço
            if (currentOrder.Id == 0) // Nova ordem
            {
                await _orderService.AddOrder(currentOrder);
            }
            else // Atualizar ordem existente
            {
                await _orderService.UpdateOrder(currentOrder);
            }

            MessageBox.Show("Ordem salva com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            Close(); // Fechar a janela após salvar
        }
    }
}
