using HumanResources.Controllers;
using System.ComponentModel;

namespace HumanResources.Models
{
    public class AbsencePostModel
    {
        public string? Status { get; set; } = "Not approved";

        public int TypeOfAbsence { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string? Reason { get; set; }
    }
}
