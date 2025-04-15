using System;
using System.Collections.Generic;
using System.IO;
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
using DocumentFormat.OpenXml.Spreadsheet;
using log4net;
using Microsoft.Win32;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Services.Interfaces;
using ServiceOrder.Services.Services;
using ServiceOrder.Utils;

namespace ServiceOrder
{
    /// <summary>
    /// Interação lógica para OptionsListView.xam
    /// </summary>
    public partial class OptionsListView : UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(OptionsListView));

        private readonly ISpreadsheetService _spreadsheetService;
        private readonly IClientService _clientService;
        private readonly IElectricCompanyService _electricCompanyService;
        private readonly IOrderService _orderService;

        public OptionsListView(
            ISpreadsheetService spreadsheetService, 
            IClientService clientService, 
            IElectricCompanyService electricCompanyService, 
            IOrderService orderService)
        {
            _spreadsheetService = spreadsheetService;
            _clientService = clientService;
            _electricCompanyService = electricCompanyService;
            _orderService = orderService;

            InitializeComponent();  
        }

        // TODO: preciso adicionar um loading, need to imporve performance and organize the code
        private async void OnSeedClick(object sender, RoutedEventArgs e)
        {
            var result = DialogUtils.ShowConfirmation("Confirmação", $"Deseja realmente fazer uma carga massiva? Atenção, isso pode gerar duplicidades.");
            if (result)
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Planilhas Excel (*.xlsx)|*.xlsx",
                    Title = "Selecione a planilha para importação"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;

                    try
                    {
                        ChangeViewOnLoad(false);

                        var spreadsheetRows = _spreadsheetService.MassiveImportFromSpreadsheet(filePath);
                        if (spreadsheetRows.Any())
                        {
                            var orders = await _orderService.GetAllAsync();
                            var clients = await _clientService.GetAllAsync();
                            var electricCompanies = await _electricCompanyService.GetAllAsync();

                            List<Client> clientsToInsert = new List<Client>(); 
                            List<ElectricCompany> companiesToInsert = new List<ElectricCompany>(); 

                            foreach (var row in spreadsheetRows)
                            {
                                string name = row.Order.Client.Name;
                                var client = clients.FirstOrDefault(c => c.Name == name);
                                if (!String.IsNullOrEmpty(name) && client == null && !clientsToInsert.Any(c => c.Name == name))
                                {
                                    client = new Domain.Entities.Client { Name = name };
                                    clientsToInsert.Add(client);
                                }

                                name = row.Order.FinalClient.Name;
                                client = clients.FirstOrDefault(c => c.Name == name);
                                if (!String.IsNullOrEmpty(name) && client == null && !clientsToInsert.Any(c => c.Name == name))
                                {
                                    client = new Domain.Entities.Client { Name = name };
                                    clientsToInsert.Add(client);
                                }

                                name = row.Order.ElectricCompany.Name;
                                var electricCompany = electricCompanies.FirstOrDefault(c => c.Name == name);
                                if (!String.IsNullOrEmpty(name) && electricCompany == null && !companiesToInsert.Any(c => c.Name == name))
                                {
                                    electricCompany = new Domain.Entities.ElectricCompany { Name = name };
                                    companiesToInsert.Add(electricCompany);
                                }
                            }

                            foreach (var client in clientsToInsert)
                                await _clientService.AddAsync(client);

                            foreach (var company in companiesToInsert)
                                await _electricCompanyService.AddAsync(company);

                            // reload entities
                            clients = await _clientService.GetAllAsync();
                            electricCompanies = await _electricCompanyService.GetAllAsync();

                            foreach (var row in spreadsheetRows)
                            {
                                row.Order.ClientId = (int)(clients.FirstOrDefault(c => c.Name == row.Order.Client.Name)?.Id);
                                row.Order.FinalClientId = (int)(clients.FirstOrDefault(c => c.Name == row.Order.FinalClient.Name)?.Id);
                                row.Order.ElectricCompanyId = (int)(electricCompanies.FirstOrDefault(c => c.Name == row.Order.ElectricCompany.Name)?.Id);
                            
                                if (orders.Any(o => o.OrderName == row.Order.OrderName))
                                {
                                    _log.Warn($"A ordem {row.Order.OrderName} já existe. Ignorando a importação.");
                                    continue;
                                }

                                await _orderService.AddOrder(row.Order);
                            }
                            ChangeViewOnLoad(true);

                            // Aqui você pode inserir no banco ou atualizar a tela
                            DialogUtils.ShowInfo("Importação Concluída", $"{spreadsheetRows.Count} projetos importados com sucesso!");
                        }
                        else
                        {
                            ChangeViewOnLoad(true);

                            DialogUtils.ShowInfo("Importação Concluída", $"Não foram indentificados registros para importar!");
                        }
                    }
                    catch (Exception ex)
                    {
                        ChangeViewOnLoad(true);

                        _log.Error("Erro ao importar a planilha.", ex);
                        DialogUtils.ShowInfo("Erro",$"Ocorreu um erro ao importar a planilha. Verifique o arquivo e tente novamente: {ex.Message}");
                        return;
                    }
                }
            }
        }

        private void ChangeViewOnLoad(bool show)
        {
            LoadingProgressBar.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
        }

        private async void OnBackupClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"Backup_serviceorders_{DateTime.Now:yyyyMMddHHmmss}.db",
                DefaultExt = ".db",
                Filter = "Banco de Dados SQLite (*.db)|*.db"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string backupPath = dialog.FileName;

                try
                {
                    ChangeViewOnLoad(false);

                    await Task.Run(() =>
                    {
                        string sourcePath = "serviceorders.db";

                        if (!File.Exists(sourcePath))
                            throw new FileNotFoundException("Arquivo de banco de dados original não encontrado.", sourcePath);

                        File.Copy(sourcePath, backupPath, overwrite: true);
                    });
                    ChangeViewOnLoad(true);

                    DialogUtils.ShowInfo("Sucesso", $"Backup realizado com sucesso!\nArquivo salvo em:\n{backupPath}");
                }
                catch (FileNotFoundException ex)
                {
                    ChangeViewOnLoad(true);

                    _log.Error("Arquivo de banco de dados não encontrado para backup.", ex);
                    DialogUtils.ShowInfo("Erro", "O arquivo original do banco de dados não foi encontrado. Verifique se ele existe.");
                }
                catch (UnauthorizedAccessException ex)
                {
                    ChangeViewOnLoad(true);

                    _log.Error("Erro de permissão ao tentar salvar backup.", ex);
                    DialogUtils.ShowInfo("Erro", "Permissão negada para salvar o arquivo no local selecionado.");
                }
                catch (Exception ex)
                {
                    ChangeViewOnLoad(true);

                    _log.Error("Erro inesperado ao realizar o backup.", ex);
                    DialogUtils.ShowInfo("Erro", "Ocorreu um erro inesperado ao realizar o backup.\n\n" + ex.Message);
                }
            }
        }
    }
}
