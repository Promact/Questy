using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Account
{
    public class ResetPassword
    {
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]

        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*[\W]).{8,14})+$", ErrorMessage = "Password must be alphanumeric including at least 1 uppercase letter,1 lowercase letter and a special character with 8 to 14 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "New password and confirm password do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }

    }
}