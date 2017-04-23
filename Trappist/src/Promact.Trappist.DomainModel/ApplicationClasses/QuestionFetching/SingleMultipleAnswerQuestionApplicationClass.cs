using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses.SingleMultipleAnswerQuestionApplicationClass
{
    /// <summary>
    /// Application class for SingleMultipleAnswerQuestion
    /// </summary>
    public class SingleMultipleAnswerQuestionApplicationClass : QuestionBase
    {
        public ICollection<SingleMultipleAnswerQuestionOption> SingleMultipleAnswerQuestionOption { get; set; }
    }
}
