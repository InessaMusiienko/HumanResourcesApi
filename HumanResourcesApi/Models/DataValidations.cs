using Microsoft.Identity.Client;

namespace HumanResourcesApi.Models
{
    public class DataValidations
    {
        public const string isRequired = "This field is required.";

        //Employee
        public const int FirstNameMinLength = 2;
        public const int FirstNameMaxLength = 50;

        public const int LastNameMinLength = 3;
        public const int LastNameMaxLength = 50;

        public const int ContactNumberMinLength = 7;
        public const int ContactNumberMaxLength = 15;

        public const int EmailMinLenght = 4;
        public const int EmailMaxLenght = 62;

        public const int AdressMinLength = 2;
        public const int AdressMaxLength = 100;

        //Department
        public const int DepartmentMaxLength = 15;
    }
}
