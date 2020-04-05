using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithTempFolder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Path.GetTempPath());
            var temp = Path.GetTempPath();

            Directory.CreateDirectory(temp + "tmp12345");
            var tmpDir = Directory.Exists(temp + "temp12345") ? $"{temp}\\temp12345" : Directory.CreateDirectory(temp + "tmp12345").FullName;
            File.Create($"{tmpDir}\\test12345.txt");
            Console.WriteLine($"{temp}");


            Console.ReadKey();

        }
    }
}
