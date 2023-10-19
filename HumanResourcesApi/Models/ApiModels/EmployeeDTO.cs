using HumanResourcesApi.Models.Entities;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using static HumanResourcesApi.Models.DataValidations;

namespace HumanResourcesApi.Models.ApiModels
{
    public class EmployeeDTO
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = isRequired)]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = isRequired)]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = isRequired)]
        [MaxLength(ContactNumberMaxLength)]
        public string ContactNumber { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = isRequired)]
        [RegularExpression("^\\S+@\\S+\\.\\S+$")]
        public string Email { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = isRequired)]
        public string Department { get; set; } = null!;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = isRequired)]
        public string JobTitle { get; set; } = null!;

        //public DateTime HireDate { get; set; }

        [MaxLength(AdressMaxLength)]
        public string Adress { get; set; } = null!;
    }
}
