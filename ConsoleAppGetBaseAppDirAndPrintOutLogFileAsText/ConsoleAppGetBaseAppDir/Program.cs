using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleAppGetBaseAppDir
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            var logFile=new FileStream($"{path}\\flqt.log",FileMode.Open,FileAccess.Read);
            var txt = string.Empty;
            using (var stream = new StreamReader(logFile,Encoding.UTF8))
            {
                txt = stream.ReadToEnd();
            }

            Console.WriteLine("Hello World!" + System.AppDomain.CurrentDomain.BaseDirectory);
            StringBuilder sb=new StringBuilder();
            
            sb.Append("#### LOG FILE #### START ####");
            sb.Append(txt);
            sb.Append("#### END ####");
            Console.WriteLine($"#### LOG FILE #### START #### {sb} #### END ####");


            var outTextByte = Encoding.UTF8.GetBytes(sb.ToString());
            var outText = Convert.ToBase64String(outTextByte);

            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
