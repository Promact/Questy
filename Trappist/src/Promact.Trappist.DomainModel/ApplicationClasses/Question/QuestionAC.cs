using Promact.Trappist.DomainModel.Models.Question;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class QuestionAC
    {
        public Models.Question.Question Question { get; set; }
        public SingleMultipleAnswerQuestion SingleMultipleAnswerQuestion { get; set; }
        public CodeSnippetQuestionAC CodeSnippetQuestionAC { get; set; }
    }
}
