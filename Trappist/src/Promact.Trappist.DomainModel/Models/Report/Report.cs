using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Report
{
    public class Report : BaseModel
    {
        [Required]
        public int TotalMarksScored { get; set; }

        [Required]
        public int Percentage { get; set; }

        [Required]
        public int Percentile { get; set; }
        
        public TestStatus TestStatus { get; set; }

        public decimal TimeTakenByAttendee { get; set; }

        public int TestAttendeeId { get; set; }

        [ForeignKey("TestAttendeeId")]
        public virtual TestAttendees TestAttendee { get; set; }

    }
}