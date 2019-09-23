using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidatorClassTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var objToValidate=new Customer();
            objToValidate.FirstName = "Hello";

            var objContext = new ValidationContext(objToValidate, null, null);
            var result =new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(objToValidate, objContext, result, true);

            foreach (var str in result)
            {
                Console.WriteLine($"ErrorMessage:{str.ErrorMessage}");
            }

            Console.ReadKey();
        }
    }
}
