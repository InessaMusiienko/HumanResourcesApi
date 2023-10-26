namespace HumanResourcesApi.Models.ApiModels
{
    public class ProjectDTO
    {
        public string ProjectName { get; set; } = null!;

        public int Duration { get; set; }

        public DateTime? StartedOn { get; set; }

        public string Status { get; set; } = null!;
    }
}
