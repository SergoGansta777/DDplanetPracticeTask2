using System.ComponentModel.DataAnnotations;

namespace Practice_task2.Model;

public partial class ManagementCompany
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Management company name is required")]
    public string Name { get; set; } = null!;

    // public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
