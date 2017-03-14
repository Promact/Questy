using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class SingleMultipleAnswerQuestion : QuestionBase
    {
        public virtual ICollection<SingleMultipleAnswerQuestionOption> SingleMutipleAnswerQuestionOption { get; set; }
    }
}
