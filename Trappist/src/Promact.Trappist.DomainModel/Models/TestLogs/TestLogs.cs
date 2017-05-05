using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.TestLogs
{
    public class TestLogs : BaseModel
    {
        [Required]
        public DateTime VisitTestLink { get; set; }

        [Required]
        public DateTime FillRegistrationForm { get; set; }

        [Required]
        public DateTime PassInstructionpage { get; set; }

        [Required]
        public DateTime StartTest { get; set; }

        [Required]
        public DateTime? FinishTest { get; set; }

        public DateTime? AwayFromTestWindow { get; set; }

        public DateTime? DisconnectedFromServer { get; set; }

        public DateTime? CloseWindowWithoutFinishingTest { get; set; }

        public DateTime? ResumeTest { get; set; }

        public int TestAttendeeId { get; set; }

        [ForeignKey("TestAttendeeId")]
        public virtual TestConduct.TestAttendees TestAttendee { get; set; }
    }
}
