using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses
{
    public class SingleMultipleQuestion
    {
        public SingleMultipleAnswerQuestion singleMultipleAnswerQuestion { get; set; }
        public List<SingleMultipleAnswerQuestionOption> singleMultipleAnswerQuestionOption { get; set; }
    }
}
