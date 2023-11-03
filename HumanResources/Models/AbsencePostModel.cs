using HumanResources.Controllers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanResources.Models
{
    public class AbsencePostModel
    {
        public string? Status { get; set; } = "Not approved";

        public int TypeOfAbsence { get; set; }

        [Required]
        public string StartDate { get; set; }

        [Required]
        public string EndDate { get; set; }

        public string? Reason { get; set; }
    }
}
