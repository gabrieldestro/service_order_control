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

namespace ServiceOrder
{
    /// <summary>
    /// Interação lógica para OptionsListView.xam
    /// </summary>
    public partial class OptionsListView : UserControl
    {
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
                        File.Copy(sourcePath, backupPath, overwrite: true);
                    });

                    MessageBox.Show($"Backup realizado com sucesso! Arquivo salvo em: {backupPath}",
                        "Backup", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao realizar o backup: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
