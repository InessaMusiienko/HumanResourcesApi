using HumanResources.Controllers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanResources.Models
{
    public class AbsenceViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Status { get; set; } = "Not approved";

        [DisplayName("Type")]
        //public string TypeOfAbsence { get; set; } = null!;
        public Types TypeOfAbsence { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string? Reason { get; set; }
    }
}
