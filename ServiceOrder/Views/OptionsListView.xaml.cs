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
using log4net;

namespace ServiceOrder
{
    /// <summary>
    /// Interação lógica para OptionsListView.xam
    /// </summary>
    public partial class OptionsListView : UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(OptionsListView));

        public OptionsListView()
        {
            InitializeComponent();
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
                    await Task.Run(() =>
                    {
                        string sourcePath = "serviceorders.db";

                        if (!File.Exists(sourcePath))
                            throw new FileNotFoundException("Arquivo de banco de dados original não encontrado.", sourcePath);

                        File.Copy(sourcePath, backupPath, overwrite: true);
                    });

                    MessageBox.Show($"Backup realizado com sucesso!\nArquivo salvo em:\n{backupPath}",
                        "Backup", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (FileNotFoundException ex)
                {
                    _log.Error("Arquivo de banco de dados não encontrado para backup.", ex);
                    MessageBox.Show("O arquivo original do banco de dados não foi encontrado. Verifique se ele existe.",
                        "Arquivo não encontrado", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (UnauthorizedAccessException ex)
                {
                    _log.Error("Erro de permissão ao tentar salvar backup.", ex);
                    MessageBox.Show("Permissão negada para salvar o arquivo no local selecionado.",
                        "Permissão negada", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    _log.Error("Erro inesperado ao realizar o backup.", ex);
                    MessageBox.Show("Ocorreu um erro inesperado ao realizar o backup.\n\n" + ex.Message,
                        "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
