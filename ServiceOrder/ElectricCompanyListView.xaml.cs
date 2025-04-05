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
    /// Interação lógica para ElectricCompanyListView.xam
    /// </summary>
    public partial class ElectricCompanyListView : UserControl
    {
        private readonly IElectricCompanyService _companyService;
        public ObservableCollection<ElectricCompany> Companies { get; set; }

        public ElectricCompanyListView(IElectricCompanyService companyService)
        {
            InitializeComponent();
            _companyService = companyService;
            Companies = new ObservableCollection<ElectricCompany>();
            LoadCompaniesAsync();
        }

        private async void LoadCompaniesAsync(Func<ElectricCompany, bool> filter = null)
        {
            try
            {
                Companies.Clear();
                await Task.Delay(500);

                var companies = await Task.Run(() => _companyService.GetAllAsync()); 
                var filteredCompanies = filter != null ? companies.Where(filter) : companies;

                foreach (var company in filteredCompanies)
                    Companies.Add(company);

                ElectricCompanyDataGrid.ItemsSource = Companies;
                CompanyRecordCountLabel.Content = $"{Companies.Count} companhia(s) exibida(s).";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar companhias: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnFilterClick(object sender, RoutedEventArgs e)
        {
            string searchText = SearchNameTextBox.Text.ToLower();
            string searchCnpjText = SearchCnpjTextBox.Text;

            LoadCompaniesAsync(company =>
                (string.IsNullOrEmpty(searchCnpjText) || company.Cnpj.ToLower().Contains(searchText) == true) &&
                (string.IsNullOrEmpty(searchText) || company.Name.ToLower().Contains(searchText) == true));
        }

        private void OnClearFiltersClick(object sender, RoutedEventArgs e)
        {
            SearchNameTextBox.Clear();
            SearchCnpjTextBox.Clear();
            LoadCompaniesAsync();
        }

        private void OnNewCompanyClick(object sender, RoutedEventArgs e)
        {
            var detailView = App.ServiceProvider.GetRequiredService<ElectricCompanyDetailView>();
            detailView.Closed += (s, args) => LoadCompaniesAsync();
            detailView.ShowDialog();
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ElectricCompany selected)
            {
                var detailView = App.ServiceProvider.GetRequiredService<ElectricCompanyDetailView>();
                detailView.SetELectricCompany(selected);
                detailView.Closed += (s, args) => LoadCompaniesAsync();
                detailView.ShowDialog();
            }
        }

        private async void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ElectricCompany selected)
            {
                var result = MessageBox.Show($"Deseja excluir a companhia '{selected.Name}'?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await _companyService.DeleteAsync(selected.Id);
                    LoadCompaniesAsync();
                }
            }
        }
    }
}
