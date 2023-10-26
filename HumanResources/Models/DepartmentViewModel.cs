using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanResources.Models
{
    public class DepartmentViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Department Name")]
        public string DepartmentName { get; set; } = null!;
    }
}
