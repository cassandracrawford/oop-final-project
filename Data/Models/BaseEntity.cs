using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Models;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }
}
