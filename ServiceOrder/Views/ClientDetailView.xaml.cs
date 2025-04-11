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
using log4net;
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
        private static readonly ILog _log = LogManager.GetLogger(typeof(ClientDetailView));

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
            try
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

                    if (!string.IsNullOrEmpty(client.Cpf))
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

                DataContext = _client;
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao configurar os dados do cliente na tela de detalhes.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao carregar os dados do cliente.");
            }
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var name = ClientNameTextBox.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    DialogUtils.ShowInfo("Erro", "Informe o nome do cliente.");
                    return;
                }

                var cpf = CpfTextBox.Text.Trim();
                var cnpj = CnpjTextBox.Text.Trim();

                _client.Cnpj = cnpj;
                _client.Cpf = cpf;
                _client.Name = name;
                _client.Description = DescriptionTextBox.Text.Trim();
                _client.LastUpdated = DateTime.Now;

                bool result;
                if (_client.Id > 0)
                {
                    result = await _clientService.UpdateAsync(_client);
                }
                else
                {
                    result = await _clientService.AddAsync(_client);
                }

                if (!result)
                {
                    DialogUtils.ShowInfo("Erro", "Erro ao salvar o cliente.");
                    return;
                }

                DialogUtils.ShowInfo("Sucesso", "Cliente salvo com sucesso!");
                Close();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao salvar cliente.", ex);
                DialogUtils.ShowInfo("Erro", "Ocorreu um erro ao salvar o cliente.");
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PersonTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CnpjPanel != null && CpfPanel != null)
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
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao alternar tipo de pessoa (CPF/CNPJ).", ex);
            }
        }

        private void CnpjTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void CnpjTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var textBox = (TextBox)sender;
                string onlyDigits = new string(textBox.Text.Where(char.IsDigit).ToArray());

                if (string.IsNullOrEmpty(onlyDigits))
                    return;

                string formatted = FormatUtils.FormatCnpj(onlyDigits);

                textBox.Text = formatted;
                textBox.CaretIndex = formatted.Length;
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao formatar CNPJ.", ex);
            }
        }

        private void CpfTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var txt = CpfTextBox;
                string formatted = FormatUtils.FormatCpf(txt.Text);

                txt.Text = formatted;
                txt.CaretIndex = txt.Text.Length;
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao formatar CPF.", ex);
            }
        }

        private void CpfTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }
    }
}
