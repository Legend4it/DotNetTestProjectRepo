using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace FindAppByRegKey
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine($"Hello World!{IsOfficeInstalled()}");




            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        private static bool IsOfficeInstalled()
        {
            //Find App regkey by name ...!
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\excel.exe");
            if (key != null)
            {
                Console.WriteLine(key);
                key.Close();
            }
            return key != null;
        }
    }
}
