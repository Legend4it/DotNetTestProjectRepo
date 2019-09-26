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

            var str = new[] {
                "123,456,789.00",
                "123.456.789,00",
                "123 456 789,00",
                "123'456'789,00",
                "123,456,789'00",
                "123,456,789,00",
                "123'456'789'00",
                "123 456 789 00",
                "123 456 789'00" };



            str.ToList().ForEach(nr => ParseDecimalWithSymbol(nr));

            Console.ReadKey();
        }
        public static decimal ParseDecimalWithSymbol(string input)
        {
            var culturListInApp = new List<string> { "en-US", "sv-SE", "de-DE", "en-SE" }; //Language list installed in Application
            var separator = Regex.Replace(input, @"[\d-]", string.Empty).Distinct().ToList();
            var culs = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(d => d.NumberFormat.NumberGroupSeparator.Contains(separator.First()) && d.NumberFormat.NumberDecimalSeparator.Contains(separator.Last()) && culturListInApp.Contains(d.Name))
                .ToDictionary(d => d.Name, d => d.NumberFormat);

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
            Console.WriteLine($"Parse Error: {input} Nr of match Culture: {count}");

            return result;
        }
        public class AppCulture
        {
            public CultureInfo Result { get; set; }
        }
    }

}
