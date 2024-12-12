using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Models
{
    // ReportItem model class for report data
    public class ReportItem
    {
        public int RentalId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string EquipmentDetails { get; set; }
        public decimal RentalCost { get; set; }
    }
}
