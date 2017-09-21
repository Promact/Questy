using Promact.Trappist.DomainModel.ApplicationClasses.Test;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class QuestionAC
    {
        public QuestionDetailAC Question { get; set; }

        public SingleMultipleAnswerQuestionAC SingleMultipleAnswerQuestion { get; set; }

        public CodeSnippetQuestionAC CodeSnippetQuestion { get; set; }

    }
}
