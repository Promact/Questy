using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.TestConduct
{
    public class TestAttendees : BaseModel
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }

        [StringLength(15)]
        public string ContactNumber { get; set; }

        [Required]
        public string RollNumber { get; set; }

        public int TestID { get; set; }

        [ForeignKey("TestID")]
        public virtual Test.Test Test { get; set; }
    }
}
