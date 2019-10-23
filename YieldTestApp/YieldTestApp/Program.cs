using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YieldTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");



            foreach (var item in FunctionWithYield(2, 10))
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        private static IEnumerable<int> FunctionWithYield(int start, int number)
        {
            for (int i = 0; i < number; i++)
            {
                yield return start + 2 * i;
            }
        }
    }
}
