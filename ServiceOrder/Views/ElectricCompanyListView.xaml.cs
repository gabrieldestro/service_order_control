﻿using System;
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
    /// Interação lógica para ElectricCompanyListView.xam
    /// </summary>
    public partial class ElectricCompanyListView : UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ElectricCompanyListView));

        private readonly IElectricCompanyService _companyService;
        public ObservableCollection<ElectricCompany> Companies { get; set; }

        public ElectricCompanyListView(IElectricCompanyService companyService)
        {
            InitializeComponent();
            _companyService = companyService;
            Companies = new ObservableCollection<ElectricCompany>();
            LoadCompaniesFilteredAsync();
        }

        private async void LoadCompaniesAsync(Func<ElectricCompany, bool> filter = null)
        {
            try
            {
                ChangeViewOnLoad(false);

                Companies.Clear();

                var companies = await Task.Run(() => _companyService.GetAllAsync());
                var filteredCompanies = filter != null ? companies.Where(filter) : companies;

                foreach (var company in filteredCompanies)
                    Companies.Add(company);

                ElectricCompanyDataGrid.ItemsSource = Companies;
                CompanyRecordCountLabel.Content = $"{Companies.Count} companhia(s) exibida(s).";
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao carregar companhias elétricas.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao carregar companhias elétricas.");
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

        private void LoadCompaniesFilteredAsync()
        {
            string searchText = SearchNameTextBox.Text.ToLower();
            string searchCnpjText = SearchCnpjTextBox.Text;

            LoadCompaniesAsync(company =>
                (string.IsNullOrEmpty(searchCnpjText) || company?.Cnpj?.ToLower().Contains(searchCnpjText) == true) &&
                (string.IsNullOrEmpty(searchText) || company?.Name?.ToLower().Contains(searchText) == true));
        }

        private void OnFilterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadCompaniesFilteredAsync();
            }
            catch (Exception ex)
            {
                _log.Warn("Erro ao aplicar filtro de companhias elétricas.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao aplicar o filtro.");
            }
        }

        private void OnClearFiltersClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SearchNameTextBox.Clear();
                SearchCnpjTextBox.Clear();
                LoadCompaniesFilteredAsync();
            }
            catch (Exception ex)
            {
                _log.Warn("Erro ao limpar filtros de companhias elétricas.", ex);
            }
        }

        private void OnNewCompanyClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var detailView = App.ServiceProvider.GetRequiredService<ElectricCompanyDetailView>();
                detailView.Closed += (s, args) => LoadCompaniesFilteredAsync();
                detailView.ShowDialog();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao abrir tela de nova companhia elétrica.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao abrir tela de nova companhia elétrica.");
            }
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.DataContext is ElectricCompany selected)
                {
                    var detailView = App.ServiceProvider.GetRequiredService<ElectricCompanyDetailView>();
                    detailView.SetELectricCompany(selected);
                    detailView.Closed += (s, args) => LoadCompaniesFilteredAsync();
                    detailView.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao abrir tela de edição da companhia elétrica.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao abrir tela de edição da companhia elétrica.");
            }
        }

        private async void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.DataContext is ElectricCompany selected)
                {
                    var result = DialogUtils.ShowConfirmation("Confirmação", $"Deseja excluir a companhia '{selected.Name}'?");
                    if (result)
                    {
                        var success = await _companyService.DeleteAsync(selected.Id);
                        if (!success)
                        {
                            DialogUtils.ShowInfo("Erro", "Erro ao excluir a companhia.");
                        }

                        LoadCompaniesFilteredAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao excluir companhia elétrica.", ex);
                DialogUtils.ShowInfo("Erro", "Erro ao excluir companhia elétrica.");
            }
        }
    }
}
