using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.TestConduct
{
    public class AttendeeAnswers : BaseModel
    {
        public string Answers { get; set; }

        public double TimeElapsed { get; set; }

        [ForeignKey("Id")]
        public virtual TestAttendees TestAttendees { get; set; }
    }
}
