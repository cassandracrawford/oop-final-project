using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Models
{
    public interface IEmployee
    {
        int EmployeeID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Password { get; set; }
        string Status { get; set; }

        string GetDetails();
    }
    public interface ICustomer
    {
        int CustomerID { get; set; }
        string PhoneNum { get; set; }
        string EMail { get; set; }
        string ClientStatus { get; set; }
        float Discount { get; set; }

        string GetDetails();
    }

    public interface IRentalEquipment
    {
        int EquipmentId { get; set; }
        Category Category { get; set; }
        string Description { get; set; }
        string Status { get; set; }
        decimal DailyRate { get; set; }
    }

    public interface ICategory
    {
        IEnumerable<RentalEquipment> GetEquipmentsByCategory();
    }
}
