using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanResources.Models
{
    public class DepartmentViewModel
    {
        [Required]
        [DisplayName("Department Name")]
        public string DepartmentName { get; set; } = null!;
    }
}
