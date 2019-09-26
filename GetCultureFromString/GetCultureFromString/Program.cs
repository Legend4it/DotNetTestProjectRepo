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

            var str = new[] { "123,456,789.00", "123 456 789,00", "123'456'789,00", "123'456'789'00", "123 456 789 00", "123 456 789'00" };



            str.ToList().ForEach(nr => ParseDecimalWithSymbol(nr));

            Console.ReadKey();
        }
        public static void TryParseDecimals(List<string> str)
        {
            var culture = new CultureInfo("en-US");
            NumberStyles style = NumberStyles.AllowLeadingWhite
                                   | NumberStyles.AllowLeadingSign
                                   | NumberStyles.AllowThousands
                                   | NumberStyles.AllowDecimalPoint
                                   | NumberStyles.AllowTrailingSign
                                   | NumberStyles.AllowTrailingWhite
                                   ;
            foreach (var item in str)
            {
                Console.WriteLine(decimal.Parse(item, style, culture));
            }
        }
        public static decimal ParseDecimalWithSymbol(string input)
        {
            var separator = Regex.Replace(input, @"[\d-]", string.Empty).Distinct().ToList();
            var culs = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(d => d.NumberFormat.NumberGroupSeparator.Contains(separator.First()) && d.NumberFormat.NumberDecimalSeparator.Contains(separator.Last()))
                .ToDictionary(d=>d.Name, d=>d.NumberFormat);
            
            decimal result = 0;
            var count = 0;
            try
            {
                foreach (var ci in culs)
                {
                    result = decimal.Parse(input, ci.Value);
                    Console.WriteLine($"{input} =={ci.Key}==> {result}");
                    count++;
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine($"{input} : {e.Message}");
            }
            Console.WriteLine($"################### {count} ################");


            //var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
            //    .GroupBy(d => d.NumberFormat)
            //    .ToDictionary(d => d.Key, d => d.First());

            //var culture = cultures.Where(d => Regex.Replace(input, @"[\d-]", string.Empty).Contains(d.Key.NumberDecimalSeparator)&& Regex.Replace(input, @"[\d-]", string.Empty).Contains(d.Key.NumberGroupSeparator)).ToList();

            //decimal result = 0;
            //var count=0;
            //try
            //{
            //    foreach (var ci in culture)
            //    {
            //        result = decimal.Parse(input, ci.Value);
            //        Console.WriteLine($"{input} =={ci.Value}==> {result}");
            //        count++;
            //    }
            //}
            //catch (FormatException e)
            //{
            //    Console.WriteLine($"{input} : {e.Message}");
            //}
            //Console.WriteLine($"################### {count} ################");
            return result;
        }
        public static decimal ParseDecimalNumber(string str, AppCulture culture)
        {

            string groupSeparators = "'.," + "¤";
            string decimalPoints = ".,'" + "¤";
            string s = str.Replace('1', '¤');

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
