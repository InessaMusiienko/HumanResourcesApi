using System;
using System.Collections.Generic;

namespace HumanResourcesApi.Models.Entities;

public partial class Project
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = null!;

    public int Duration { get; set; }

    public DateTime? StartedOn { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<EmployeeProject> EmployeesProjects { get; set; } = new List<EmployeeProject>();
}
