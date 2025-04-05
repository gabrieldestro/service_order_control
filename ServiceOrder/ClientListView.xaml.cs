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
        private readonly IClientService _clientService;
        public ObservableCollection<Client> Clients { get; set; }

        public ClientListView(IClientService clientService)
        {
            InitializeComponent();
            _clientService = clientService;
            Clients = new ObservableCollection<Client>();
            LoadClientsAsync();
        }

        private async void LoadClientsAsync(Func<Client, bool> filter = null)
        {
            try
            {
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
                MessageBox.Show($"Erro ao carregar clientes: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnFilterClick(object sender, RoutedEventArgs e)
        {
            string searchText = SearchNameTextBox.Text.ToLower();
            string searchCnpjText = SearchCnpjTextBox.Text;

            LoadClientsAsync(client =>
                (string.IsNullOrEmpty(searchCnpjText) || client.Cnpj.ToLower().Contains(searchText) == true) &&
                (string.IsNullOrEmpty(searchText) || client.Name.ToLower().Contains(searchText) == true));
        }

        private void OnClearFiltersClick(object sender, RoutedEventArgs e)
        {
            SearchNameTextBox.Clear();
            SearchCnpjTextBox.Clear();
            LoadClientsAsync();
        }

        private void OnNewClientClick(object sender, RoutedEventArgs e)
        {
            var detailView = App.ServiceProvider.GetRequiredService<ClientDetailView>();
            detailView.Closed += (s, args) => LoadClientsAsync();
            detailView.ShowDialog();
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Client selected)
            {
                var detailView = App.ServiceProvider.GetRequiredService<ClientDetailView>();
                detailView.SetClient(selected);
                detailView.Closed += (s, args) => LoadClientsAsync();
                detailView.ShowDialog();
            }
        }

        private async void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Client selected)
            {
                var result = MessageBox.Show($"Deseja excluir o cliente '{selected.Name}'?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await _clientService.DeleteAsync(selected.Id);
                    LoadClientsAsync();
                }
            }
        }
    }
}
