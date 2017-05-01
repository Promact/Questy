using Promact.Trappist.DomainModel.Enum;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.ApplicationClasses.TestConduct
{
    public class TestAnswerAC
    {
        [Required]
        public int QuestionId { get; set; }

        public int[] OptionChoice { get; set; }

        public string Code { get; set; }

        public QuestionStatus QuestionStatus { get; set; }
    }
}
