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
using ServiceOrder.Domain.Entities;
using ServiceOrder.Repository.Context;
using ServiceOrder.Services.Interfaces;

namespace ServiceOrder
{
    /// <summary>
    /// Lógica interna para ClientDetailView.xaml
    /// </summary>
    public partial class ClientDetailView : Window
    {
        private readonly IClientService _clientService;

        public ClientDetailView(IClientService clientService)
        {
            InitializeComponent();

            _clientService = clientService;

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
        }

        public void SetClient(Client client)
        {
            if (client == null)
            {
                client = new Client
                {
                    CreatedDate = DateTime.Now,
                    LastUpdated = DateTime.Now
                };
            }

            DataContext = client;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            var name = ClientNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Informe o nome do cliente.");
                return;
            }

            _clientService.AddAsync(new Client
            {
                Name = name
            });

            MessageBox.Show("Cliente salvo com sucesso!");
            this.Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
