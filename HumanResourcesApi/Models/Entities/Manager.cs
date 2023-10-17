using System;
using System.Collections.Generic;

namespace HumanResourcesApi.Models.Entities;

public partial class Manager
{
    public int EmployeeId { get; set; }

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;
}
