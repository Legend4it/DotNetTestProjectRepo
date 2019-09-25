using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GetCultureFromString
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture.ClearCachedData();
            var thread = new Thread(
                s => ((AppCulture)s).Result = Thread.CurrentThread.CurrentCulture);
            var state = new AppCulture();
            thread.Start(state);
            thread.Join();
            var culture = state.Result;

            Console.WriteLine(culture.NumberFormat.NumberDecimalSeparator);
            Console.WriteLine(culture.NumberFormat.NumberGroupSeparator);

            var str = new[] { "123,456,789.00", "123'456'789,00", "123'456'789'00", "123 456 789,00", "123 456 789 00", "123 456 789'00" };


            var obj = new
            {
                culture.NumberFormat.NumberDecimalSeparator,
                culture.NumberFormat.NumberGroupSeparator,
                culture.NumberFormat.DigitSubstitution,
                culture.NumberFormat.NumberGroupSizes,
                culture.NumberFormat.NumberDecimalDigits
            };


            foreach (var nr in str)
            {
                Console.WriteLine(nr);
                Decimal value = ParseDecimalNumber(nr, state);
                Console.WriteLine(value.ToString() + "\n");
            }

            Console.ReadKey();
            Console.ReadKey();
        }
        public static decimal ParseDecimalNumber(string str, AppCulture culture)
        {

            string groupSeparators = "'.," + "¤";
            string decimalPoints = ".,'" + "¤";
            string s = str.Replace('1','¤');

            NumberFormatInfo nfi = (NumberFormatInfo)culture.Result.NumberFormat.Clone();
            NumberStyles style = NumberStyles.AllowLeadingWhite
                                   | NumberStyles.AllowLeadingSign
                                   | NumberStyles.AllowThousands
                                   | NumberStyles.AllowDecimalPoint
                                   | NumberStyles.AllowTrailingSign
                                   | NumberStyles.AllowTrailingWhite
                                   ;
            decimal value = 0;
            bool parsed = false;

            for (int i = 0; !parsed && i < groupSeparators.Length; ++i)
            {

                nfi.NumberGroupSeparator = groupSeparators.Substring(i, 1);

                for (int j = 0; !parsed && j < decimalPoints.Length; ++j)
                {
                    if (groupSeparators[i] == decimalPoints[j]) continue; // skip when group and decimal separator are identical

                    nfi.NumberDecimalSeparator = decimalPoints.Substring(j, 1);

                    parsed = Decimal.TryParse(s, style, nfi, out value);

                }
            }

            if (!parsed) throw new ArgumentOutOfRangeException("s");

            return value;

        }
        public class AppCulture
        {
            public CultureInfo Result { get; set; }
        }
    }

}
