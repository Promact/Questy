using Promact.Trappist.DomainModel.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.TestConduct
{
    public class TestCodeSolution: BaseModel
    {
        public int TestAttendeeId { get; set; }

        public int QuestionId { get; set; }

        public string Solution { get; set; }

        public ProgrammingLanguage Language { get; set; }

        public double Score { get; set; }

        [ForeignKey("TestAttendeeId")]
        public virtual TestAttendees TestAttendee { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question.Question Question { get; set; }

        public virtual ICollection<TestCaseResult> TestCaseResultCollection { get; set; }
    }
}