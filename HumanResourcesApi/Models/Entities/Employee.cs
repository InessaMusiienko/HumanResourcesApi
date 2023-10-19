using System;
using System.Collections.Generic;

namespace HumanResourcesApi.Models.Entities;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int JobTitleId { get; set; }
    public virtual JobTitle JobTitle { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public string Adress { get; set; } = null!;

    public int DepartmentId { get; set; }
    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual ICollection<EmployeeProject> EmployeesProjects { get; set; } = new List<EmployeeProject>();
}
