using Promact.Trappist.DomainModel.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.TestConduct
{
    public class TestConduct : BaseModel
    {
        public int TestAttendeeId { get; set; }

        public int QuestionId { get; set; }

        public QuestionStatus QuestionStatus { get; set; }

        [ForeignKey("TestAttendeeId")]
        public virtual TestAttendees TestAttendees { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question.Question Question { get; set; }
    }
}
