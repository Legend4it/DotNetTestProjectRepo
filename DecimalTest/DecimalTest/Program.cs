using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DecimalTest
{
    class Program
    {
        static void Main(string[] args)
        {


            var styles = NumberStyles.Currency;
            PrintOutFormat("123,456,789.00", styles);

            var styles1 = NumberStyles.Currency | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
            PrintOutFormat("123,456,789.00", styles1);
            
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        private static void PrintOutFormat(string input, NumberStyles styles)
        {
            
            Console.WriteLine(decimal.Parse(input, styles, CultureInfo.InvariantCulture));
            Console.WriteLine(decimal.Parse(input, styles, CultureInfo.InvariantCulture));

            Console.WriteLine(decimal.Parse(input, styles, CultureInfo.CurrentCulture));
            Console.WriteLine(decimal.Parse(input, styles, CultureInfo.CurrentCulture));

            Console.WriteLine("##########################");

        }
    }
}
