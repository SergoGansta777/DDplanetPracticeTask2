using System.ComponentModel.DataAnnotations;

namespace Practice_task2.Model;

public partial class Location
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Location name is required")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Management company id is required")]
    public int? ManagementCompanyId { get; set; }

    //public virtual ICollection<AddressInLocation> AddressInLocations { get; set; } = new List<AddressInLocation>();

    // public virtual ManagementCompany? ManagementCompany { get; set; }
}
