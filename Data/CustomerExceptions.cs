using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data
{
    // Base class for customer related exceptions
    public class CustomerExceptions : Exception
    {
        public CustomerExceptions(string message) : base(message) { }
    }

    // Specific excepctions for invalid name, email, and contanct number
    public class InvalidFirstNameException : CustomerExceptions
    {
        public InvalidFirstNameException() : base("Error: First Name is required.") { }
    }

    public class InvalidLastNameException : CustomerExceptions
    {
        public InvalidLastNameException() : base("Error: Last Name is required.") { }
    }
    public class InvalidEmailException : CustomerExceptions
    {
        public InvalidEmailException() : base("Error: Email address is not in a valid format.") { }
    }
    public class InvalidPhoneException : CustomerExceptions
    {
        public InvalidPhoneException() : base("Error: Phone number is not in a valid format.") { }
    }
}
