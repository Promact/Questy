using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup
{
    public class ConnectionString
    {
        [Required]
        public string Value { get; set; }
    }
}