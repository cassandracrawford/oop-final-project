using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Models
{
    public abstract class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Abstract class to be implemented to Customer and Employee
        public abstract string GetDetails();

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
