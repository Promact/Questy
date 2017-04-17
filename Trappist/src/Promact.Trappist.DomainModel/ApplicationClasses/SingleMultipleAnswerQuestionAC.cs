using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses
{
    public class SingleMultipleAnswerQuestionAC
    {
        public SingleMultipleAnswerQuestion SingleMultipleAnswerQuestion { get; set; }
        public List<SingleMultipleAnswerQuestionOptionAC> SingleMultipleAnswerQuestionOption { get; set; }
    }
}
