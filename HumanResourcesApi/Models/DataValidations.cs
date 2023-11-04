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

        //Absence
        public const int StatusMaxLength = 20;
        public const int TypeOfAbsenceMaxLength = 15;
        public const int ReasonMaxLength = 100;

        //JobTitle
        public const int JobNameMaxLength = 27;

        //Account
        public const string EmailRegEx = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";

        public const int MinPasswordLength = 6;

        public const string PaswordRegEx = @"^(?=.*\d)(?=.*[a-z])(?=.*[a-zA-Z]).{6,}$";

        public const int UserNamesMinLength = 2;
    }
}
