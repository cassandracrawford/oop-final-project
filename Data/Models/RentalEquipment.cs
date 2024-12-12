using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models;

public class RentalEquipment: BaseEntity, IRentalEquipment
{
    int _equipmentId;
    Category _category;
    string _description;
    string _status;
    decimal _dailyRate;

    public int EquipmentId { get => _equipmentId; set => _equipmentId = value; }
    
    public string Description { get => _description; set => _description = value; }
    
    public string Status { get => _status; set => _status = value; }
    
    public decimal DailyRate { get => _dailyRate; set => _dailyRate = value; }

    public Category Category { get => _category; set => _category = value; }

    public int CategoryId { get; set; }

    public RentalEquipment() { }
}
