using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;

namespace Promact.Trappist.Repository.Questions
{
    public interface IQuestionsRespository
    {
        /// <summary>
        /// Get all questions
        /// </summary>
        /// <returns>Question list</returns>
        List<SingleMultipleAnswerQuestion> GetAllQuestions();

        /// <summary>
        /// Add code snippet question to the database
        /// </summary>
        /// <returns>
        /// 0 -> Add operation failed
        /// 1 -> Add operation successful
        /// </returns>
        int AddCodeSnippetQuestion(CodeSnippetQuestionModel codeSnippetQuestionModel);
    }
}
