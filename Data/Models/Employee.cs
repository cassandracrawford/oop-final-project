using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Models
{
    public class Employee : Person, IEmployee
    {
        public int EmployeeID { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }

        public Employee() { }

        public Employee(int employeeID, string firstName, string lastName, string password, string status)
        {
            EmployeeID = employeeID;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Status = status;
        }

        // Implements the abstract method, Getting the details of Employee
        public override string GetDetails()
        {
            return $"Employee ID: {EmployeeID}, Name: {GetFullName()}, Status: {Status}";
        }
    }
}
