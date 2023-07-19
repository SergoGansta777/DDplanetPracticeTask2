using System.ComponentModel.DataAnnotations;

namespace Practice_task2.Model;

public partial class AddressInLocation
{
    public string? FiasHouseCode { get; set; }

    public string? FiasStreetCode { get; set; }

    public string? FiasCityCode { get; set; }

    public string? FiasRegionCode { get; set; }

    [Required(ErrorMessage = "Location Id is required")]
    public int? LocationId { get; set; }

    //public virtual Location? Location { get; set; }
}
