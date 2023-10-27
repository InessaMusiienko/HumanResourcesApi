using HumanResources.Models.Enums;

namespace HumanResources.Models
{
    public class AllEmployeeQueryModel
    {
        public AllEmployeeQueryModel()
        {
            this.CurrentPage = 1;
            this.EmployeePerPage = 4;
        }
        public int EmployeePerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalEmployees { get; set; }
        public IEnumerable<AllEmployeeViewModel> Employees { get; set; } = null!;
        public EmployeeSorting Sorting { get; set; }
        public string? SearchTerm { get; set; }
    }
}
