using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulate
{
    internal class Customer
    {
        public string FirstName { get; set; }

        public string LastName { get; set; } 

        public int Age { get; set; }

        public int Id { get; set; }
    }
}
