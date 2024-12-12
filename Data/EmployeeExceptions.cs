using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data
{
    // Base class for employee related exceptions
    public class EmployeeExceptions : Exception
    {
        public EmployeeExceptions(string message) : base(message) { }
    }

    // Specific exception for invalid login details
    public class InvalidLoginDetailsException : EmployeeExceptions
    {
        public InvalidLoginDetailsException() : base("Invalid Employee ID or Password.") { }
    }
}
