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
using ServiceOrder.Domain.Utils;
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
                DialogUtils.ShowInfo("Erro", "Erro ao carregar a lista de ordens finalizadas.");
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

            if (!string.IsNullOrEmpty(_orderDeadline.OrderIdentifier))
                OrderComboBox.SelectedValue = _orderDeadline.OrderIdentifier;
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
                ProjectSpecificCheckBox.IsEnabled = false;

                if (!string.IsNullOrEmpty(deadline.OrderIdentifier))
                {
                    ProjectSpecificCheckBox.IsChecked = true;
                    OrderComboBox.Visibility = Visibility.Visible;
                }
                else
                {
                    ProjectSpecificCheckBox.IsChecked = false;
                    OrderComboBox.Visibility = Visibility.Collapsed;
                }

                DescriptionTextBox.Text = deadline.Description;

                DocumentSentDaysTxt.Text = deadline.DocumentSentDays?.ToString();
                DocumentReceivedDaysTxt.Text = deadline.DocumentReceivedDays?.ToString();
                ProjectRegistrationDaysTxt.Text = deadline.ProjectRegistrationDays?.ToString();
                ProjectSubmissionDaysTxt.Text = deadline.ProjectSubmissionDays?.ToString();
                ProjectApprovalDaysTxt.Text = deadline.ProjectApprovalDays?.ToString();
                InspectionRequestDaysTxt.Text = deadline.InspectionRequestDays?.ToString();
                FinalizationDaysTxt.Text = deadline.FinalizationDays?.ToString();
                PaymentDaysTxt.Text = deadline.PaymentDays?.ToString();
            }

            DataContext = _orderDeadline;
        }

        private void ProjectSpecificCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            OrderComboBox.Visibility = Visibility.Visible;
        }

        private void ProjectSpecificCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            OrderComboBox.Visibility = Visibility.Collapsed;
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

                if (ProjectSpecificCheckBox.IsChecked == true)
                {
                    if (OrderComboBox.SelectedItem is Order selectedOrder)
                    {
                        _orderDeadline.OrderIdentifier = selectedOrder.OrderName;
                    }
                    else
                    {
                        DialogUtils.ShowInfo("Erro", "Prazos para projetos específicos precisam ter um projeto selecionado.");
                        return;
                    }
                }
                else
                {
                    _orderDeadline.OrderIdentifier = null;
                }

                _orderDeadline = currentDeadline;
                _orderDeadline.Description = DescriptionTextBox.Text.Trim();
                _orderDeadline.LastUpdated = DateTime.Now;
                _orderDeadline.CreatedDate = DateTime.Now;

                _orderDeadline.DocumentSentDays = DocumentSentDaysTxt.Text.ToIntOrNull();
                _orderDeadline.DocumentReceivedDays = DocumentReceivedDaysTxt.Text.ToIntOrNull();
                _orderDeadline.ProjectRegistrationDays = ProjectRegistrationDaysTxt.Text.ToIntOrNull();
                _orderDeadline.ProjectSubmissionDays = ProjectSubmissionDaysTxt.Text.ToIntOrNull();
                _orderDeadline.ProjectApprovalDays = ProjectApprovalDaysTxt.Text.ToIntOrNull();
                _orderDeadline.InspectionRequestDays = InspectionRequestDaysTxt.Text.ToIntOrNull();
                _orderDeadline.FinalizationDays = FinalizationDaysTxt.Text.ToIntOrNull();
                _orderDeadline.PaymentDays = PaymentDaysTxt.Text.ToIntOrNull();

                if (!_orderDeadlineService.AllDeadlinesRegistered(_orderDeadline))
                {
                    DialogUtils.ShowInfo("Erro", "Todos os campos de prazo devem ter valor!");
                    return;
                }

                var identifier = _orderDeadline.OrderIdentifier;
                if (await _orderDeadlineService.HasDeadline(identifier))
                {
                    DialogUtils.ShowInfo("Erro", "Já existe um prazo cadastrado para essa condição!");
                    return;
                }

                var success = false;
                if (_orderDeadline.Id > 0)
                {
                    success = await _orderDeadlineService.UpdateAsync(_orderDeadline);
                }
                else
                {
                    success = await _orderDeadlineService.AddAsync(_orderDeadline);
                }

                if (success)
                    DialogUtils.ShowInfo("Sucesso", "Prazo salvo com sucesso!");
                else
                    DialogUtils.ShowInfo("Erro", "Erro ao salvar prazo!");

                Close();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao salvar prazo.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao salvar o prazo.");
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
