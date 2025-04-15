using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ServiceOrder.Formatters
{
    public class DaysFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "-";

            if (value is int intVal)
                return $"{intVal} dia{(intVal == 1 ? "" : "s")}";

            if (int.TryParse(value.ToString(), out int parsed))
                return $"{parsed} dia{(parsed == 1 ? "" : "s")}";

            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
