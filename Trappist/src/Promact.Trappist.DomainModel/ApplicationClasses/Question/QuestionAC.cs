namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class QuestionAC
    {
        public Models.Question.Question Question { get; set; }
        public SingleMultipleAnswerQuestionAC SingleMultipleAnswerQuestionAC { get; set; }
        public CodeSnippetQuestionAC CodeSnippetQuestionAC { get; set; }
    }
}
