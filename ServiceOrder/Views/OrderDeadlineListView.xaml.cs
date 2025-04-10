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
    /// Interação lógica para OrderDeadlineListView.xam
    /// </summary>
    public partial class OrderDeadlineListView : UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(OrderDeadlineListView));

        private readonly IOrderDeadlineService _service;
        public ObservableCollection<OrderDeadline> Deadlines { get; set; }

        public OrderDeadlineListView(IOrderDeadlineService service)
        {
            InitializeComponent();
            _service = service;
            Deadlines = new ObservableCollection<OrderDeadline>();
            LoadDeadlinesWithFiltersAsync();
        }

        private async void LoadDeadlinesAsync(Func<OrderDeadline, bool> filter = null)
        {
            try
            {
                ChangeViewOnLoad(false);

                Deadlines.Clear();

                var deadlines = await Task.Run(() => _service.GetAllAsync());
                var filtered = filter != null ? deadlines.Where(filter) : deadlines;

                foreach (var d in filtered)
                    Deadlines.Add(d);

                DataGrid.ItemsSource = Deadlines;
                RecordCountLabel.Content = $"{Deadlines.Count} prazo(s) exibido(s).";
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao carregar prazos.", ex);
                MessageBox.Show($"Erro ao carregar prazos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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
            string searchText = SearchNameTextBox.Text.ToLower();

            LoadDeadlinesWithFiltersAsync();
        }

        private void LoadDeadlinesWithFiltersAsync()
        {
            string searchText = SearchNameTextBox.Text.ToLower();
            LoadDeadlinesAsync(deadline =>
                string.IsNullOrEmpty(searchText) || (deadline.OrderId?.ToLower().Contains(searchText) ?? false));
        }   

        private void OnClearFiltersClick(object sender, RoutedEventArgs e)
        {
            SearchNameTextBox.Clear();
            LoadDeadlinesWithFiltersAsync();
        }

        private void OnNewDeadlineClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var detailView = App.ServiceProvider.GetRequiredService<OrderDeadlineDetailView>();
                detailView.SetDeadline(null); // Novo prazo
                detailView.Closed += (s, args) => LoadDeadlinesWithFiltersAsync();
                detailView.ShowDialog();
            }
            catch (Exception ex)
            {
                _log.Error("Erro ao abrir tela de novo prazo.", ex);
                MessageBox.Show("Erro ao abrir a tela de novo prazo.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is OrderDeadline selected)
            {
                try
                {
                    var detailView = App.ServiceProvider.GetRequiredService<OrderDeadlineDetailView>();
                    detailView.SetDeadline(selected);
                    detailView.Closed += (s, args) => LoadDeadlinesWithFiltersAsync();
                    detailView.ShowDialog();
                }
                catch (Exception ex)
                {
                    _log.Error("Erro ao abrir tela de edição de prazo.", ex);
                    MessageBox.Show("Erro ao abrir a tela de edição de prazo.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is OrderDeadline selected)
            {
                var result = MessageBox.Show($"Deseja excluir o prazo da ordem '{selected.OrderId}'?",
                    "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _service.DeleteAsync(selected.Id);
                        LoadDeadlinesWithFiltersAsync();
                    }
                    catch (Exception ex)
                    {
                        _log.Error($"Erro ao excluir o prazo da ordem '{selected.OrderId}'.", ex);
                        MessageBox.Show("Erro ao excluir o prazo.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
