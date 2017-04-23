using Promact.Trappist.DomainModel.Models.Enums;
using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class SingleMultipleChoiceQuestion
    {
        public string QuestionDetail { get; set; }
        public QuestionType QuestionType { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public IList<SingleMultipleChoiceQuestionAnswer> SingleMultipleChoiceQuestionAnswer { get; set; }
    }
}
