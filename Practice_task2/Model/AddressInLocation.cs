﻿using System.ComponentModel.DataAnnotations;

namespace Practice_task2.Model;

public partial class AddressInLocation
{
    public int Id { get; set; }

    [StringLength(4, ErrorMessage = "FiasHouseCode must be a string with a maximum length of 4")]
    public string? FiasHouseCode { get; set; }

    [StringLength(4, ErrorMessage = "FiasStreetCode must be a string with a maximum length of 4")]
    public string? FiasStreetCode { get; set; }

    [StringLength(3, ErrorMessage = "FiasCityCode must be a string with a maximum length of 3")]
    public string? FiasCityCode { get; set; }

    [StringLength(2, ErrorMessage = "FiasRegionCode must be a string with a maximum length of 2")]
    public string? FiasRegionCode { get; set; }

    [Required(ErrorMessage = "Location Id is required")]
    public int? LocationId { get; set; }

    //public virtual Location? Location { get; set; }
}
