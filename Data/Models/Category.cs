namespace FinalProject.Data.Models;

public class Category : BaseEntity, ICategory
{
    public int CategoryId { get; set; }

    // Navigation property for related RentalEquipment
    public IEnumerable<RentalEquipment> Equipments { get; set; }

    // Override interface method
    public IEnumerable<RentalEquipment> GetEquipmentsByCategory() => Equipments;
}
