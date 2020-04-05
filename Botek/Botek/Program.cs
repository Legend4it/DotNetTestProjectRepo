using System;
using System.Threading;

namespace Botek
{
    class Program
    {
        static void Main(string[] args)
        {

            Timer taskTimer = new Timer(HardwareHandlerStart, null, 0, 2000);
            Console.ReadLine();
        }

        private static void HardwareHandlerStart(object sender)
        {
            var task = new HardwareHandlerTask();

            //Console.WriteLine($"{task.TaskId} {task.Result.Name}");
        }
    }
}
