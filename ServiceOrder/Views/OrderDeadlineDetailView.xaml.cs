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
using ServiceOrder.Domain.Entities;
using ServiceOrder.Services.Interfaces;
using ServiceOrder.Services.Services;
using ServiceOrder.Utils;

namespace ServiceOrder
{
    /// <summary>
    /// Lógica interna para OrderDeadlineDetailView.xaml
    /// </summary>
    public partial class OrderDeadlineDetailView : Window
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(OrderDeadlineDetailView));

        private readonly IOrderDeadlineService _orderDeadlineService;
        private readonly IOrderService _orderService;

        private OrderDeadline _orderDeadline = new OrderDeadline();
        private List<Order> _orders = new List<Order>();

        public OrderDeadlineDetailView(
            IOrderDeadlineService orderDeadlineService,
            IOrderService orderService)
        {
            InitializeComponent();

            _orderDeadlineService = orderDeadlineService;
            _orderService = orderService;

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();

            Loaded += OrderDeadlineDetailView_Loaded;
        }

        private async void OrderDeadlineDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await LoadOrdersAsync();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao carregar ordens finalizadas para os prazos.", ex);
                MessageBox.Show("Erro ao carregar a lista de ordens finalizadas.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadOrdersAsync()
        {
            var orders = await _orderService.GetPendingOrdersAsync();
            _orders = orders.OrderBy(o => o.OrderName)
                            .ToList();

            OrderComboBox.ItemsSource = _orders;
            OrderComboBox.DisplayMemberPath = "OrderName";
            OrderComboBox.SelectedValuePath = "OrderName";

            if (!string.IsNullOrEmpty(_orderDeadline.OrderId))
                OrderComboBox.SelectedValue = _orderDeadline.OrderId;
        }

        public void SetDeadline(OrderDeadline deadline)
        {
            if (deadline == null)
            {
                _orderDeadline = new OrderDeadline();
            }
            else
            {
                _orderDeadline = deadline;
                OrderComboBox.IsEnabled = false;
            }

            DataContext = _orderDeadline;
        }

        private void OnlyPositiveNumbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void OnlyPositiveNumbers_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;

            string onlyDigits = new string(textBox.Text.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(onlyDigits))
                return;

            textBox.Text = onlyDigits;
            textBox.CaretIndex = onlyDigits.Length;
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is not OrderDeadline currentDeadline)
                    throw new InvalidOperationException("Objeto de prazo inválido.");

                if (OrderComboBox.SelectedItem is Order selectedOrder)
                {
                    _orderDeadline.OrderId = selectedOrder.OrderName;
                }

                _orderDeadline = currentDeadline;
                _orderDeadline.Description = DescriptionTextBox.Text.Trim();
                _orderDeadline.LastUpdated = DateTime.Now;

                if (_orderDeadline.Id > 0)
                {
                    await _orderDeadlineService.UpdateAsync(_orderDeadline);
                }
                else
                {
                    await _orderDeadlineService.AddAsync(_orderDeadline);
                }

                MessageBox.Show("Prazo salvo com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao salvar prazo.", ex);
                MessageBox.Show("Erro ao salvar o prazo.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
