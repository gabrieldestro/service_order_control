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
        public Order Order { get; set; }
        private readonly IOrderService _orderService;

        // Injeção de dependência do IOrderService
        public OrderDetailView(IOrderService orderService)
        {
            InitializeComponent();

            _orderService = orderService;

            // Caso tenha recebido uma ordem, inicialize os campos
            if (Order != null)
            {
                ClienteTextBox.Text = Order.Name;
                DescricaoTextBox.Text = Order.Description;
                DataCriacaoTextBox.Text = Order.CreateDate.ToString("g");
            }
            else
            {
                DataCriacaoTextBox.Text = DateTime.Now.ToString("g");
            }
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (Order == null)
            {
                Order = new Order
                {
                    Name = ClienteTextBox.Text,
                    Description = DescricaoTextBox.Text,
                    CreateDate = DateTime.Now
                };
                await _orderService.AddOrder(Order); // Salva nova ordem no repositório
            }
            else
            {
                Order.Name = ClienteTextBox.Text;
                Order.Description = DescricaoTextBox.Text;
                await _orderService.UpdateOrder(Order); // Atualiza ordem existente
            }

            MessageBox.Show("Ordem salva com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
