using Promact.Trappist.DomainModel.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Promact.Trappist.DomainModel.ApplicationClasses.CodeSnippet;

namespace Promact.Trappist.DomainModel.ApplicationClasses.TestConduct
{
    public class TestAnswerAC
    {
        [Required]
        public int QuestionId { get; set; }

        public IEnumerable<int> OptionChoice { get; set; }

        public Code Code { get; set; }

        public bool IsAnswered { get; set; }

        public QuestionStatus QuestionStatus { get; set; }
    }
}
