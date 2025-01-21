using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ControlzEx.Theming;
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

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();

            _orderService = orderService;
            Orders = new ObservableCollection<Order>();
            LoadOrdersAsync();
        }

        private void OnNewRegistrationClick(object sender, RoutedEventArgs e)
        {
            var detailView = App.ServiceProvider.GetRequiredService<OrderDetailView>();
            detailView.SetOrder(null); // Nova ordem (criação)
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

            // Obtém a lista de itens e encontra o registro com a menor data de atualização
            var items = OrderDataGrid.Items.Cast<Order>(); // Substitua 'Order' pelo tipo real do item.
            var oldestItem = items.OrderByDescending(o => o.LastUpdated).FirstOrDefault();

            if (oldestItem != null)
            {
                var detailView = App.ServiceProvider.GetRequiredService<OrderDetailView>();
                detailView.SetOrder(oldestItem); // Ordem existente (edição)
                detailView.Closed += (s, args) => LoadOrdersAsync();
                detailView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nenhum registro encontrado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Order selectedOrder)
            {
                var detailView = App.ServiceProvider.GetRequiredService<OrderDetailView>();
                detailView.SetOrder(selectedOrder); // Ordem existente (edição)
                detailView.Closed += (s, args) => LoadOrdersAsync();
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
                    LoadOrdersAsync();
                }
            }
        }

        private async void LoadOrdersAsync(Func<Order, bool> filter = null)
        {
            try
            {
                // Limpar a coleção existente
                Orders.Clear();

                // Mostrar o indicador de carregamento
                ChangeViewOnLoad(false);

                await Task.Delay(1000); // Atraso de 1 segundos para simulação de carregamento

                // Carregar os dados em background
                var orders = await Task.Run(() => _orderService.GetAllAsync());

                // Aplicar o filtro, se presente
                var filteredOrders = filter != null ? orders.Where(filter) : orders;

                // Adicionar as ordens filtradas à lista
                foreach (var order in filteredOrders)
                {
                    Orders.Add(order);
                }

                // Atualizar a fonte de dados do DataGrid
                OrderDataGrid.ItemsSource = Orders;
                
                // Atualizar o contador de registros exibidos
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
            BackupButton.IsEnabled = show;

            if (!show) LoadingProgressBar.Visibility = Visibility.Visible;
            else LoadingProgressBar.Visibility = Visibility.Collapsed;  
        }

        private void OnFilterClick(object sender, RoutedEventArgs e)
        {
            // Obter valores dos filtros
            string searchText = SearchTextBox.Text.ToLower();
            string searchIdText = SearchIdTextBox.Text;
            string status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            LoadOrdersAsync(order =>
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
            LoadOrdersAsync();
        }
        private void UpdateRecordCount(int count)
        {
            RecordCountLabel.Content = count == 1
                ? $"{count} registro exibido."
                : $"{count} registros exibidos.";
        }

        private async void OnBackupClick(object sender, RoutedEventArgs e)
        {
            // Exibir o diálogo para o usuário selecionar o local do backup
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"Backup_serviceorders_{DateTime.Now:yyyyMMddHHmmss}.db",
                DefaultExt = ".db",
                Filter = "Banco de Dados SQLite (*.db)|*.db"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string backupPath = dialog.FileName;

                try
                {
                    // Mostrar indicador de carregamento
                    ChangeViewOnLoad(false);

                    // Simular atraso para salvar o banco
                    await Task.Run(() =>
                    {
                        string sourcePath = "serviceorders.db"; // Caminho original
                        File.Copy(sourcePath, backupPath, overwrite: true); // Realizar o backup
                    });

                    // Ocultar indicador de carregamento
                    ChangeViewOnLoad(true);

                    // Exibir mensagem de sucesso
                    MessageBox.Show($"Backup realizado com sucesso! Arquivo salvo em: {backupPath}",
                                    "Backup", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Ocultar indicador de carregamento
                    ChangeViewOnLoad(true);

                    // Exibir mensagem de erro
                    MessageBox.Show($"Erro ao realizar o backup: {ex.Message}",
                                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
