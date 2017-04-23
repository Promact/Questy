using System.ComponentModel.DataAnnotations;
namespace Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup
{
    public class BasicSetup
    {
        [Required]
        [StringLength(150)]
        [EmailAddress]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public EmailSettings EmailSettings { get; set; }
        public ConnectionStringParamters ConnectionString { get; set; }
    }
}
