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
            if (input.Length > 11)
                input = input.Substring(0, 11);

            return input.Length switch
            {
                <= 3 => input,
                <= 6 => $"{input[..3]}.{input[3..]}",
                <= 9 => $"{input[..3]}.{input[3..6]}.{input[6..]}",
                _ => $"{input[..3]}.{input[3..6]}.{input[6..9]}-{input[9..]}",
            };
        }
    }
}
