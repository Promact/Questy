using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses.SingleMultipleAnswerQuestionAC
{
    /// <summary>
    /// Application class for SingleMultipleAnswerQuestion
    /// </summary>
    public class SingleMultipleAnswerQuestionApplicationClass : Models.Question.Question
    {
        public ICollection<SingleMultipleAnswerQuestionOption> SingleMultipleAnswerQuestionOption { get; set; }
    }
}
