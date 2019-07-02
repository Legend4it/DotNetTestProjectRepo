using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionTest
{
    public class ExceptionImpl:Exception
    {
        public ExceptionImpl(string message):base(message)
        {
            Console.WriteLine(message);
        }

        public ExceptionImpl(string message, Exception innerException):base(message,innerException)
        {
            
            switch (innerException.GetType().ToString())
            {
                case "System.IO.IOException":
                    HandelException(message, innerException);
                    break;
                case "System.Exception":
                    HandelException(message, innerException);
                    break;
                default:
                    HandelException("######################",innerException);
                    break;
            }
        }

        private static void HandelException(string message, Exception innerException)
        {
            Console.WriteLine($"{message} {innerException}");
            //Log Exception in Log4Net!!!!!
        }
    }
}
