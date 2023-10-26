using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanResources.Models
{
    public class AbsenceViewModel
    {
        [Key]
        public int AbsenceId { get; set; }

        public string Status { get; set; } = "Not approved";

        [DisplayName("Type")]
        public int TypeOfAbsence { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Reason { get; set; }
    }
}
