using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Services.Interfaces;

namespace ServiceOrder
{
    /// <summary>
    /// Interação lógica para ClientListView.xam
    /// </summary>
    public partial class ClientListView : UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ClientListView));

        private readonly IClientService _clientService;
        public ObservableCollection<Client> Clients { get; set; }

        public ClientListView(IClientService clientService)
        {
            InitializeComponent();
            _clientService = clientService;
            Clients = new ObservableCollection<Client>();
            LoadClientsFilteredAsync();
        }

        private async void LoadClientsAsync(Func<Client, bool> filter = null)
        {
            try
            {
                ChangeViewOnLoad(false);

                Clients.Clear();
                await Task.Delay(500); // Simulação de carregamento

                var clients = await Task.Run(() => _clientService.GetAllAsync());
                var filteredClients = filter != null ? clients.Where(filter) : clients;

                foreach (var client in filteredClients)
                    Clients.Add(client);

                ClientDataGrid.ItemsSource = Clients;
                ClientRecordCountLabel.Content = $"{Clients.Count} cliente(s) exibido(s).";
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao carregar lista de clientes.", ex);
                MessageBox.Show($"Erro ao carregar clientes!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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

            LoadingProgressBar.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }

        private void OnFilterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadClientsFilteredAsync();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao aplicar filtros de cliente.", ex);
                MessageBox.Show("Erro ao aplicar filtros.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadClientsFilteredAsync()
        {
            string searchText = SearchNameTextBox.Text.ToLower();
            string searchCnpjText = SearchCnpjTextBox.Text;

            LoadClientsAsync(client =>
                (string.IsNullOrEmpty(searchCnpjText) || client?.Cnpj?.ToLower().Contains(searchCnpjText) == true) &&
                (string.IsNullOrEmpty(searchText) || client?.Name?.ToLower().Contains(searchText) == true));
        }

        private void OnClearFiltersClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SearchNameTextBox.Clear();
                SearchCnpjTextBox.Clear();
                LoadClientsFilteredAsync();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao limpar filtros de cliente.", ex);
                MessageBox.Show("Erro ao limpar filtros.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnNewClientClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var detailView = App.ServiceProvider.GetRequiredService<ClientDetailView>();
                detailView.Closed += (s, args) => LoadClientsFilteredAsync();
                detailView.ShowDialog();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao abrir janela de novo cliente.", ex);
                MessageBox.Show("Erro ao abrir tela de cadastro de cliente.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.DataContext is Client selected)
                {
                    var detailView = App.ServiceProvider.GetRequiredService<ClientDetailView>();
                    detailView.SetClient(selected);
                    detailView.Closed += (s, args) => LoadClientsFilteredAsync();
                    detailView.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao abrir janela de edição de cliente.", ex);
                MessageBox.Show("Erro ao abrir tela de edição.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.DataContext is Client selected)
                {
                    var result = MessageBox.Show($"Deseja excluir o cliente '{selected.Name}'?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        bool success = await _clientService.DeleteAsync(selected.Id);
                        if (!success)
                        {
                            MessageBox.Show("Erro ao excluir cliente.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        LoadClientsFilteredAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao excluir cliente.", ex);
                MessageBox.Show("Erro ao excluir cliente.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
