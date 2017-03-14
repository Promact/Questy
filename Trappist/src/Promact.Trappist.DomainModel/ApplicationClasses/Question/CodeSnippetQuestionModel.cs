using Promact.Trappist.DomainModel.Models.Enums;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class CodeSnippetQuestionModel
    {
        public QuestionType QuestionType { get; set;}
        public DifficultyLevel  DifficultyLevel { get; set; }
        public string QuestionDetail { get; set; }

    }
}
