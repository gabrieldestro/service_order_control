using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceOrder.Domain.Utils
{
    public static class ConvertExtensions
    {
        public static int? ToIntOrNull(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return null;
        }
        public static decimal? ToDecimalFromCurrencyOrNull(this string value)
        {
            value = value.Replace("R$ ", "");
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            if (decimal.TryParse(value, out decimal result))
            {
                return result;
            }
            return null;
        }
        public static decimal? ToDecimalOrNull(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            if (decimal.TryParse(value, out decimal result))
            {
                return result;
            }
            return null;
        }
    }
}
