using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Question;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class SingleMultipleAnswerQuestionAC
    {
        public SingleMultipleAnswerQuestion SingleMultipleAnswerQuestion { get; set; }

        public List<SingleMultipleAnswerQuestionOption> SingleMultipleAnswerQuestionOption { get; set; }
    }
}
