using System;
using System.Collections.Generic;

namespace HumanResourcesApi.Models.Entities;

public partial class JobTitle
{
    public int JobTitleId { get; set; }

    public string JobName { get; set; } = null!;

    public int SalaryId { get; set; }
    public virtual Salary Salary { get; set; } = null!;

    public int WorkingHours { get; set; }

    public DateTime? StartDate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

}
