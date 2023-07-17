using System;
using System.Collections.Generic;

namespace Practice_task2.Model;

public partial class Location
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ManagementCompanyId { get; set; }

    public virtual ManagementCompany? ManagementCompany { get; set; }
}
