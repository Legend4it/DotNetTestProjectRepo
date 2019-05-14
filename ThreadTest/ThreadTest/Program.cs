using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = Task.Factory.StartNew(() => {
                Thread.CurrentThread.Name = "foo";
                Thread.Sleep(10000);   // Use Debug + Break to see it
            });

            while (!task.IsCompleted)
            {
                Console.WriteLine("Task Complete .. !");
            }

            task.Wait();

            Console.ReadLine();

        }
    }
}
