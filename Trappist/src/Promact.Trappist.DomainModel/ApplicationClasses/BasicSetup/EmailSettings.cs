using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup
{
    public class EmailSettings
    {
        [Required]
        public string Server { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string ConnectionSecurityOption { get; set; }
    }
}
