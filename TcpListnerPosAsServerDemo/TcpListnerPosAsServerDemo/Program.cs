using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpListnerPosAsServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            var cm = new ConnectionManager(11000);
            cm.Start();

            Console.ReadKey();

        }
    }
}
