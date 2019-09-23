using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidatorClassTest
{
    public class Customer
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }


    }
}
