namespace HumanResources.Models
{
    public class AbsenceAllView
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Status { get; set; } = null!;

        public string TypeOfAbsence { get; set; }

        public string StartDate { get; set; } = null!;

        public string EndDate { get; set; } = null!;

        public string? Reason { get; set; }
    }
}
