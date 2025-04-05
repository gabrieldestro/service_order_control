using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceOrder.Utils
{
    public static class FormatUtils
    {
        public static string FormatCnpj(string input)
        {
            if (input.Length > 14)
                input = input.Substring(0, 14);

            return input.Length switch
            {
                <= 2 => input,
                <= 5 => $"{input[..2]}.{input[2..]}",
                <= 8 => $"{input[..2]}.{input[2..5]}.{input[5..]}",
                <= 12 => $"{input[..2]}.{input[2..5]}.{input[5..8]}/{input[8..]}",
                _ => $"{input[..2]}.{input[2..5]}.{input[5..8]}/{input[8..12]}-{input[12..]}",
            };
        }

        public static string FormatCpf(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var digits = new string(input.Where(char.IsDigit).ToArray());

            if (digits.Length > 11)
                digits = digits.Substring(0, 11);

            return digits.Length switch
            {
                >= 10 => Convert.ToUInt64(digits).ToString(@"000\.000\.000\-00"),
                >= 7 => Convert.ToUInt64(digits).ToString(@"000\.000\.000"),
                >= 4 => Convert.ToUInt64(digits).ToString(@"000\.000"),
                _ => digits
            };
        }
    }
}
