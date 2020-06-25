using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthSort
{
    class Program
    {
        static void Main(string[] args)
        {
            var months = new List<string> {
        "April",
        "August",
        "December",
        "February",
        "January",
        "July",
        "June",
        "March",
        "May",
        "November",
        "October",
        "September"};


            //var newList = months.OrderBy(m=>DateTime.ParseExact(m, "MMMM", CultureInfo.InvariantCulture).Month);

            string monthName = new DateTime(2010, 8, 1).ToString("MMM", CultureInfo.InvariantCulture);
            Console.WriteLine(monthName);
            Console.ReadKey();
        }
    }
}
