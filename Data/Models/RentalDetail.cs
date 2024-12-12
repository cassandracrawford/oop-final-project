using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Models
{
    public class RentalDetail
    {
        // Primary Keys
        [Key]
        public int RentalID { get; set; }

        // Foreign Keys and Required Fields
        [Required]
        public string CustomerID { get; set; } = string.Empty;

        [Required]
        public string EquipmentID { get; set; } = string.Empty;

        // Dates and Times
        [Required]
        public DateTime RentalDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime ReturnDate { get; set; } = DateTime.Now;

        // Financial Information
        [Range(0, double.MaxValue)]
        public decimal DailyRate { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public decimal TotalCost { get; set; } = 0;

        // Status Tracking
        public string Status { get; set; } = "Pending"; // Pending, Active, Completed, Cancelled

        // Calculation Methods
        public decimal CalculateCost(decimal dailyRate)
        {
            // Validate inputs
            if (dailyRate <= 0)
            {
                return 0;
            }

            if (RentalDate > ReturnDate)
            {
                return 0;
            }

            try
            {
                // Calculate rental duration
                int days = (ReturnDate - RentalDate).Days + 1;

                // Calculate base cost
                decimal baseCost = dailyRate * days;

                // Store the values
                DailyRate = dailyRate;
                TotalCost = baseCost;

                return baseCost;
            }
            catch (Exception)
            {
                // Note: Consider proper error logging in production
                return 0;
            }
        }

        // Validation Methods
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(CustomerID)
                && !string.IsNullOrEmpty(EquipmentID)
                && RentalDate <= ReturnDate
                && RentalDate >= DateTime.Today;
        }

        // Status Management
        public void UpdateStatus(string newStatus)
        {
            if (IsValidStatus(newStatus))
            {
                Status = newStatus;
            }
        }

        private bool IsValidStatus(string status)
        {
            return status is "Pending" or "Active" or "Completed" or "Cancelled";
        }
    }
}