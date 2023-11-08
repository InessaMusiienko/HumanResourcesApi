using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HumanResourcesApi.Models.Entities;

public partial class Project
{
    [Key]
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = null!;

    public int Duration { get; set; }

    public DateTime StartedOn { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<EmployeeProject> EmployeesProjects { get; set; } = new List<EmployeeProject>();
}
