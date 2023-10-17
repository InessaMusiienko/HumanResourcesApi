using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static HumanResourcesApi.Models.DataValidations;

namespace HumanResourcesApi.Models.ApiModels
{
    public class DepartmentDTO
    {
        [Required(ErrorMessage = isRequired)]
        [MaxLength(DepartmentMaxLength)]
        public string DepartmentName { get; set; } = null!;
    }
}
