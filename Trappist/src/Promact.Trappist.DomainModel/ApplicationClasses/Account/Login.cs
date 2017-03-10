using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Account
{
    public class Login
    {
        
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please Enter Proper Format EmailId")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
