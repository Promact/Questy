using Promact.Trappist.DomainModel.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.DomainModel.ApplicationClasses
{
    public class SingleMultipleQuestion
    {
        public SingleMultipleAnswerQuestion singleMultipleAnswerQuestion { get; set; }
        public SingleMultipleAnswerQuestionOption[] singleMultipleAnswerQuestionOption { get; set; }
    }
}
