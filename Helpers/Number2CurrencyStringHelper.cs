using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public class Number2CurrencyStringHelper
    {
        public static string Transform(decimal numberDecimal)
        {
            var sum = (long)numberDecimal;
            var tiyin = (long)((numberDecimal - sum) * 100);

            var startPrefix = "";
            if (numberDecimal < 0)
            {
                startPrefix = "Минус ";
            }else if(numberDecimal == 0)
            {
                return "Ноль сум " + tiyin + " тийин";;
            }
            
            return startPrefix + WholePartTransform(sum) + " " + (tiyin < 10 ? "0" : "") + tiyin + " тийин";
        }

        public static string WholePartTransform(decimal numberDecimal)
        {
            numberDecimal = Math.Abs(numberDecimal);
            var number = ((long)numberDecimal).ToString();

            var len = number.Length;

            if (len % 3 != 0)
            {
                for (var i = 0; i < 3 - len % 3; i++)
                {
                    number = "0" + number;
                }
            }
            len = number.Length;
            var items = len / 3;
            var parts = new List<long>();
            var number2String = "";

            for (int i = items; i > 0; i--)
            {
                parts.Add(int.Parse(number.Substring((i - 1) * 3, 3)));
            }
            
           
            var prualization = Prualization();
            
            for (int i = 0; i < items; i++)
            {
                var part = parts[i];
                if (part > 0)
                {
                    var digitsStr = new List<string>();
                    var digits = new List<long>();
                    if (part > 99)
                    {
                        digits.Add(part / 100 * 100);
                    }

                    var mode1 = part % 100;
                    if (mode1 != 0)
                    {
                        var mode2 = part % 10;
                        var flag = i == 1 && mode1 != 11 && mode1 != 12 && mode2 < 3 ? -1 : 1;
                        if (mode1 < 20 || mode2 == 0)
                        {

                            if (mode1 > 2 && flag == -1)
                            {
                                digits.Add(mode1);
                            }
                            else
                            {
                                digits.Add(flag * mode1);
                            }

                        }
                        else
                        {
                            digits.Add(mode1 / 10 * 10);
                            digits.Add(flag * mode2);
                        }
                    }

                    var last = Math.Abs(digits.LastOrDefault());

                    foreach (var digit in digits)
                    {
                        digitsStr.Add(BaseList()[digit]);
                    }

                    var prua = prualization[i][
                        ((last %= 100) > 4 && last < 20)
                            ? 2
                            : (int) CartPrualization()[Math.Min((int) last % 10, 5)]];


                    digitsStr.Add(prua);

                    number2String = string.Join(" ", digitsStr) + " " + number2String;
                }
                else if (i == 0)
                {
                    number2String = prualization[i][(int)CartPrualization()[0]];
                }
            }

            number2String = number2String.Trim();
            return char.ToUpper(number2String[0]) + number2String.Substring(1);
        }

        public static List<long> CartPrualization()
        {
            return new List<long> { 2, 0, 1, 1, 1, 2 };
        }

        public static List<List<string>> Prualization()
        {
            return new List<List<string>>()
            {
                new List<string>
                {
                    "сум", "сума", "сум"
                },
                 new List<string>
                {
                    "тысяча", "тысячи", "тысяч"
                },
                 new List<string>
                {
                    "миллион", "миллиона", "миллионов"
                },
                 new List<string>
                {
                   "миллиард", "миллиарда", "миллиардов"
                },
                 new List<string>
                {
                    "триллион", "триллиона", "триллионов"
                },
                 new List<string>
                {
                    "квадриллион", "квадриллиона", "квадриллионов"
                }
            };
        }

        public static Dictionary<long, string> BaseList()
        {
            return new Dictionary<long, string>()
            {
                {-2,  "две"},
                {-1,  "одна"},
                {0 , "ноль"},
                {1,  "один"},
                {2,  "два"},
                {3,  "три"},
                {4,  "четыре"},
                {5,  "пять"},
                {6,  "шесть"},
                {7,  "семь"},
                {8,  "восемь"},
                {9,  "девять"},
                {10,  "десять"},
                {11,  "одиннадцать"},
                {12,  "двенадцать"},
                {13,  "тринадцать"},
                {14,  "четырнадцать"},
                {15,  "пятнадцать"},
                {16,  "шестнадцать"},
                {17,  "семнадцать"},
                {18,  "восемнадцать"},
                {19,  "девятнадцать"},
                {20,  "двадцать"},
                {30,  "тридцать"},
                {40,  "сорок"},
                {50,  "пятьдесят"},
                {60,  "шестьдесят"},
                {70,  "семьдесят"},
                {80,  "восемьдесят"},
                {90,  "девяносто"},
                {100,  "сто"},
                {200,  "двести"},
                {300,  "триста"},
                {400,  "четыреста"},
                {500,  "пятьсот"},
                {600,  "шестьсот"},
                {700,  "семьсот"},
                {800,  "восемьсот"},
                {900,  "девятьсот"},
            };
        }

        public static List<long> GetPruaIndexes()
        {
            return new List<long> { 2, 0, 1, 1, 1, 2 };
        }
    }
}
