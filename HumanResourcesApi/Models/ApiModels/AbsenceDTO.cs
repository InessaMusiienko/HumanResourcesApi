using HumanResourcesApi.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using static HumanResourcesApi.Models.DataValidations;

namespace HumanResourcesApi.Models.ApiModels
{
    public class AbsenceDTO
    {
        /*public int EmployeeId { get; set; }*/ //??

        public string EmployeeFullName { get; set; } = null!;     

        [MaxLength(StatusMaxLength)]
        public string Status { get; set; } = null!;

        [MaxLength(TypeOfAbsenceMaxLength)]
        public string TypeOfAbsence { get; set; } = null!;

        //culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"
        //Thread.CurrentThread.CurrentCulture.DateTimeFormatInfo.DateSeparator = '/'
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [MaxLength(ReasonMaxLength)]
        public string? Reason { get; set; }

    }
}
