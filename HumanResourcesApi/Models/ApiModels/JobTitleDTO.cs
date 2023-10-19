using HumanResourcesApi.Models.Entities;
using System.ComponentModel.DataAnnotations;
using static HumanResourcesApi.Models.DataValidations;

namespace HumanResourcesApi.Models.ApiModels
{
    public class JobTitleDTO
    {
        [MaxLength(JobNameMaxLength)]
        public string JobName { get; set; } = null!;

        public decimal Salary { get; set; }

        public int WorkingHours { get; set; }

        //public string? StartDate { get; set; }
    }
}
