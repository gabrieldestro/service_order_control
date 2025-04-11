using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServiceOrder.Utils
{
    public class DialogUtils
    {
        public static void ShowInfo(string title, string message, Window owner = null)
        {
            var dialog = new Views.Dialog.InfoDialog(title, message)
            {
                Owner = owner ?? Application.Current.MainWindow
            };
            dialog.ShowDialog();
        }

        public static bool ShowConfirmation(string title, string message, Window owner = null)
        {
            var dialog = new Views.Dialog.ConfirmationDialog(title, message)
            {
                Owner = owner ?? Application.Current.MainWindow
            };
            return dialog.ShowDialog() == true;
        }
    }
}
