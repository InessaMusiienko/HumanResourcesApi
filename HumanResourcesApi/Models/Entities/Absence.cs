using System;
using System.Collections.Generic;

namespace HumanResourcesApi.Models.Entities;

public partial class Absence
{
    public int AbsenceId { get; set; }

    public int EmployeeId { get; set; }

    public string Status { get; set; } = null!;

    public int TypeOfAbsence { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Reason { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
