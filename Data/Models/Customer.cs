using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Models
{
    // For managing a Customer information from the database
    public class Customer : Person, ICustomer
    {
        int customerID;
        string firstName;
        string lastName;
        string phoneNum;
        string eMail;
        string clientStatus;
        float discount;

        public int CustomerID { get => customerID; set => customerID = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string PhoneNum { get => phoneNum; set => phoneNum = value; }
        public string EMail { get => eMail; set => eMail = value; }
        public string ClientStatus { get => clientStatus; set => clientStatus = value; }
        public float Discount { get => discount; set => discount = value; }

        public Customer()
        {

        }

        public Customer(int customerID, string firstName, string lastName, string phoneNum, string eMail, string clientStatus, float discount)
        {
            this.customerID = customerID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNum = phoneNum;
            this.eMail = eMail;
            this.clientStatus = clientStatus;
            this.discount = discount;
        }

        // Implements abstract method, Gets the details of the Customer
        public override string GetDetails()
        {
            return $"Customer ID: {CustomerID}, Name: {GetFullName()}, Email: {EMail}, Status: {ClientStatus}, Discount: {Discount}%";
        }

    }
}
