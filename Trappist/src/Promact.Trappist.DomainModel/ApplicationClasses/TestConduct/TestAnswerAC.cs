using Promact.Trappist.DomainModel.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.ApplicationClasses.TestConduct
{
    public class TestAnswerAC
    {
        [Required]
        public int QuestionId { get; set; }

        public IEnumerable<int> OptionChoice { get; set; }

        public string Code { get; set; }

        public QuestionStatus QuestionStatus { get; set; }
    }
}
