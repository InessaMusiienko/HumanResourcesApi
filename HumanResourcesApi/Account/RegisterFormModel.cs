using System.ComponentModel.DataAnnotations;
using static HumanResourcesApi.Models.DataValidations;

namespace HumanResourcesApi.Account
{
    public class RegisterFormModel
    {
        [Required(ErrorMessage = "Please enter your first name")]
        [MinLength(UserNamesMinLength, ErrorMessage = "First name must be at least 2 symbols")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [MinLength(UserNamesMinLength, ErrorMessage = "Last name must be at least 2 symbols")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [RegularExpression(EmailRegEx, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        [MinLength(MinPasswordLength, ErrorMessage = "Your password should be at least 6 characters long")]
        [RegularExpression(PaswordRegEx, ErrorMessage = "Please enter a valid password with at least one digit")] //check if minlenght is auto applied
        public string Password { get; set; }

        [Required(ErrorMessage = "Please repeat password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string PasswordRepeat { get; set; }
    }
}
