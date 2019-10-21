using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionTest
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                var y = 0;
                throw new ExceptionImpl("hello",new NoNullAllowedException("InnerException"));
            }
            catch (ExceptionImpl e)
            {
                throw;
            }
        }
    }
}
