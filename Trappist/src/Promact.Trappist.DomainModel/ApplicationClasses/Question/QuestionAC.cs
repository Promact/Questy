using Promact.Trappist.DomainModel.Models.Question;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class QuestionAC
    {
        public Models.Question.Question question { get; set; }
        public SingleMultipleQuestion singleMultipleQuestion { get; set; }
        public CodeSnippetQuestion codeSnippetQuestion { get; set; }
    }
}
