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
using ServiceOrder.Repository.Context;
using ServiceOrder.Services.Interfaces;
using ServiceOrder.Services.Services;
using ServiceOrder.Utils;

namespace ServiceOrder
{
    /// <summary>
    /// Lógica interna para ElectricCompanyDetailView.xaml
    /// </summary>
    public partial class ElectricCompanyDetailView : Window
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ElectricCompanyDetailView));

        private readonly IElectricCompanyService _electricCompanyService;
        private ElectricCompany _company = new ElectricCompany();

        public ElectricCompanyDetailView(IElectricCompanyService electricCompanyService)
        {
            InitializeComponent();

            _electricCompanyService = electricCompanyService;

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
        }

        public void SetELectricCompany(ElectricCompany company)
        {
            try
            {
                if (company == null)
                {
                    _company = new ElectricCompany();
                }
                else
                {
                    _company = company;

                    CompanyNameTextBox.Text = company.Name;
                    CnpjTextBox.Text = company.Cnpj;
                    DescriptionTextBox.Text = company.Description;

                    CnpjTextBox.IsEnabled = false;
                }

                DataContext = _company;
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao carregar dados da companhia elétrica.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao carregar dados da companhia elétrica.");
            }
        }

        private async void OnSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var name = CompanyNameTextBox.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    DialogUtils.ShowInfo("Erro", "Informe o nome da companhia elétrica.");
                    return;
                }

                var cnpj = CnpjTextBox.Text.Trim();
                /*
                if (string.IsNullOrEmpty(cnpj))
                {
                    MessageBox.Show("Informe o CNPJ da companhia elétrica.");
                    return;
                }
                else if (cnpj.Length < 18)
                {
                    MessageBox.Show("CNPJ inválido.");
                    return;
                }
                */

                if (cnpj.Length > 0 && cnpj.Length < 18)
                {
                    DialogUtils.ShowInfo("Erro", "Informe um CNPJ válido.");
                    return;
                }

                _company.Name = name;
                _company.Cnpj = cnpj;
                _company.Description = DescriptionTextBox.Text.Trim();
                _company.LastUpdated = DateTime.Now;

                var success = false;
                if (_company.Id > 0)
                {
                    success = await _electricCompanyService.UpdateAsync(_company);
                }
                else
                {
                    success = await _electricCompanyService.AddAsync(_company);
                }

                if (success)
                    DialogUtils.ShowInfo("Sucesso", "Companhia elétrica salva com sucesso!");
                else
                    DialogUtils.ShowInfo("Erro", "Erro ao salvar Companhia elétrica!");

                this.Close();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao salvar companhia elétrica.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao salvar companhia elétrica.");
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                _log.Warn("Erro ao formatar CNPJ.", ex); // warning apenas, não crítico
            }
        }
    }
}
