using System;
using System.Collections.Generic;

namespace Practice_task2.Model;

public partial class ManagementCompany
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
