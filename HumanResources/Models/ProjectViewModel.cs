using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanResources.Models
{
    public class ProjectViewModel
    {
        [Key]
        public int ProjectId { get; set; }

        [DisplayName("Project Name")]
        public string ProjectName { get; set; } = null!;

        [DisplayName("Duration/weeks")]
        public int Duration { get; set; }

        [DisplayName("Started")]
        public DateTime? StartedOn { get; set; }

        public string Status { get; set; } = null!;
    }
}
