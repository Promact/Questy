using System.Collections.Generic;
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

        public bool StarredCandidate { get; set; }
        
        public int TestId { get; set; }

        [ForeignKey("TestId")]
        public virtual Test.Test Test { get; set; }

        public virtual ICollection<TestConduct> TestConduct { get; set; }

        public virtual AttendeeAnswers AttendeeAnswers { get; set; }

        public virtual Report.Report Report { get; set; }

        public bool CheckedCandidate { get; set; }

    }
}
