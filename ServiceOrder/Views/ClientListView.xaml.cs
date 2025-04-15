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
using ServiceOrder.Utils;

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
                DialogUtils.ShowInfo("Erro", $"Erro ao carregar clientes!");
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
                DialogUtils.ShowInfo("Erro", "Erro ao aplicar filtros.");
            }
        }

        private void LoadClientsFilteredAsync()
        {
            string searchText = SearchNameTextBox.Text.ToLower();
            string searchCnpjCpfText = SearchCnpjCpfTextBox.Text;

            LoadClientsAsync(client =>
                (string.IsNullOrEmpty(searchCnpjCpfText) 
                || client?.Cnpj?.ToLower().Contains(searchCnpjCpfText) == true 
                || client?.Cpf?.ToLower().Contains(searchCnpjCpfText) == true) &&
                (string.IsNullOrEmpty(searchText) || client?.Name?.ToLower().Contains(searchText) == true));
        }

        private void OnClearFiltersClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SearchNameTextBox.Clear();
                SearchCnpjCpfTextBox.Clear();
                LoadClientsFilteredAsync();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao limpar filtros de cliente.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao limpar filtros.");
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
                DialogUtils.ShowInfo("Erro", "Erro ao abrir tela de cadastro de cliente.");
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
                DialogUtils.ShowInfo("Erro", "Erro ao abrir tela de edição.");
            }
        }

        private async void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.DataContext is Client selected)
                {
                    var result = DialogUtils.ShowConfirmation("Confirmação", $"Deseja excluir o cliente '{selected.Name}'?");
                    if (result)
                    {
                        bool success = await _clientService.DeleteAsync(selected.Id);
                        if (!success)
                        {
                            DialogUtils.ShowInfo("Erro", "Erro ao excluir cliente.");
                        }

                        LoadClientsFilteredAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao excluir cliente.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao excluir cliente.");
            }
        }
    }
}
