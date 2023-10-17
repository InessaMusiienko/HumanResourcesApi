using System;
using System.Collections.Generic;

namespace HumanResourcesApi.Models.Entities;

public partial class Salary
{
    public int SalaryId { get; set; }

    public decimal Amount { get; set; }

    public virtual ICollection<JobTitle> JobTitles { get; set; } = new List<JobTitle>();
}
