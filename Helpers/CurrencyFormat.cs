using System;
using System.Globalization;
using static System.String;

namespace Helpers
{
    public class CurrencyFormat
    {
        public static string Transform(decimal number, int pow = 2, string delimiter = " ", string decimalDelimiter = ".")
        {
            var hasMinus = Math.Abs(number) > number;
            
            number = Math.Abs(number);
            number = Math.Round(number, pow);
            
            var main = (long)number;
            var foreign = (long)((number - main) * (decimal)Math.Pow(10, pow));
            var mainStr = main.ToString();
            var len = mainStr.Length;
            var residue = len % 3;
            var result = mainStr.Substring(0, residue);

            if (len > residue)
            {
                for (var i = residue; i < len; i += 3)
                {
                    result += (IsNullOrEmpty(result) ? "" : delimiter) + mainStr.Substring(i, 3);
                }
            }

            return (hasMinus ? "-" : "") + result + decimalDelimiter + AddZero(foreign,pow);
        }

        private static string AddZero(decimal number = 0, int maxNumber = 2)
        {
            var numberStr = number.ToString(CultureInfo.InvariantCulture);

            for (var i = 0; i < maxNumber - numberStr.Length; i++)
            {
                numberStr = "0" + numberStr;
            }

            return numberStr;
        }
    }
}