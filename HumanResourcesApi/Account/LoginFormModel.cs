using System.ComponentModel.DataAnnotations;
using static HumanResourcesApi.Models.DataValidations;

namespace HumanResourcesApi.Account
{
    public class LoginFormModel
    {
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [RegularExpression(EmailRegEx, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        [MinLength(MinPasswordLength, ErrorMessage = "Your password should be at least 6 characters long")]
        [RegularExpression(PaswordRegEx, ErrorMessage = "Please enter a valid password with at least one digit")]
        public string Password { get; set; }

        public string RememberMe { get; set; }
    }
}
