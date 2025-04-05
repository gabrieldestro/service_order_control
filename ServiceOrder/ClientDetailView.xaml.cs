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
using ControlzEx.Standard;
using ControlzEx.Theming;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Repository.Context;
using ServiceOrder.Services.Interfaces;
using ServiceOrder.Utils;

namespace ServiceOrder
{
    /// <summary>
    /// Lógica interna para ClientDetailView.xaml
    /// </summary>
    public partial class ClientDetailView : Window
    {
        private readonly IClientService _clientService;
        private Client _client = new Client();

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
                _client = new Client();
            }
            else
            {
                _client = client;

                ClientNameTextBox.Text = client.Name;
                CnpjTextBox.Text = client.Cnpj;
                DescriptionTextBox.Text = client.Description;

                if (!String.IsNullOrEmpty(client.Cpf))
                {
                    CpfTextBox.Text = client.Cpf;
                    PersonTypeComboBox.SelectedIndex = 1; // Física
                    CpfPanel.Visibility = Visibility.Visible;
                    CnpjPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PersonTypeComboBox.SelectedIndex = 0; // Jurídica
                    CpfPanel.Visibility = Visibility.Collapsed;
                    CnpjPanel.Visibility = Visibility.Visible;
                }

                PersonTypeComboBox.IsEnabled = false;
                CnpjTextBox.IsEnabled = false;
                CpfTextBox.IsEnabled = false;
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

            if (CnpjPanel.Visibility == Visibility.Visible)
            {
                var cnpj = CnpjTextBox.Text.Trim();
                if (string.IsNullOrEmpty(cnpj))
                {
                    MessageBox.Show("Informe o CNPJ do cliente.");
                    return;
                }
                else if (cnpj.Length < 18)
                {
                    MessageBox.Show("CNPJ inválido.");
                    return;
                }

                _client.Cnpj = cnpj;
            }

            if (CpfPanel.Visibility == Visibility.Visible)
            {
                var cpf = CpfTextBox.Text.Trim();

                if (string.IsNullOrEmpty(cpf))
                {
                    MessageBox.Show("Informe o CPF do cliente.");
                    return;
                }
                else if (cpf.Length < 14)
                {
                    MessageBox.Show("CPF inválido.");
                    return;
                }
                _client.Cpf = cpf;
            }

            _client.Name = name;
            _client.Description = DescriptionTextBox.Text.Trim();
            _client.LastUpdated = DateTime.Now;

            if (_client.Id > 0)
            {
                _clientService.UpdateAsync(_client);
            }
            else
            {
                _clientService.AddAsync(_client);
            }

            MessageBox.Show("Cliente salvo com sucesso!");
            this.Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void PersonTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PersonTypeComboBox.SelectedIndex == 0) // Jurídica
            {
                CnpjPanel.Visibility = Visibility.Visible;
                CpfPanel.Visibility = Visibility.Collapsed;
            }
            else // Física
            {
                CnpjPanel.Visibility = Visibility.Collapsed;
                CpfPanel.Visibility = Visibility.Visible;
            }
        }

        private void CnpjTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void CnpjTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;

            string onlyDigits = new string(textBox.Text.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(onlyDigits))
                return;

            string formatted = FormatUtils.FormatCnpj(onlyDigits);

            textBox.Text = formatted;
            textBox.CaretIndex = formatted.Length;
        }
        private void CpfTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = CpfTextBox;
            string formatted = FormatUtils.FormatCpf(txt.Text);

            txt.Text = formatted;
            txt.CaretIndex = txt.Text.Length;
        }
        private void CpfTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }       
    }
}
