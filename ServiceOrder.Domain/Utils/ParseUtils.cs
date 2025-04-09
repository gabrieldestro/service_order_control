using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceOrder.Domain.Utils
{
    public class ParseUtils
    {
        public static DateTime? TryParseDate(string? input)
        {
            if (DateTime.TryParse(input, out var result))
                return result;
            return null;
        }

        public static decimal? TryParseDecimal(string? input)
        {
            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
                return result;
            return null;
        }
    }
}
