using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace HumanResources.Models
{
    public class AllEmployeeViewModel
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; } = null!;

        [DisplayName("Last Name")]
        public string LastName { get; set; } = null!;

        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; } = null!;

        [DisplayName("Email")]
        public string Email { get; set; } = null!;

        [DisplayName("Department")]
        public string Department { get; set; } = null!;

        [DisplayName("Position")]
        public string JobTitle { get; set; } = null!;

        //public DateTime HireDate { get; set; }

        [DisplayName("Personal Adress")]
        public string Adress { get; set; } = null!;
        //public int AllEmployeeTotalCount { get; set; }
    }
}
