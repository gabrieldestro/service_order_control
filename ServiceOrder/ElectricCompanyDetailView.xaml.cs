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
    /// Lógica interna para ElectricCompanyDetailView.xaml
    /// </summary>
    public partial class ElectricCompanyDetailView : Window
    {
        private readonly IElectricCompanyService _electricCompanyService;

        public ElectricCompanyDetailView(IElectricCompanyService electricCompanyService)
        {
            InitializeComponent();

            _electricCompanyService = electricCompanyService;

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            var name = CompanyNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Informe o nome da companhia elétrica.");
                return;
            }

            _electricCompanyService.AddAsync(new ElectricCompany { Name = name });

            MessageBox.Show("Companhia elétrica salva com sucesso!");
            this.Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
