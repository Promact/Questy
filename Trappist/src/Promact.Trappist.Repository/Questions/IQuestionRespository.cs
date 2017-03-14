using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Questions
{
    public interface IQuestionRepository
    {
        /// <summary>
        /// Get all questions
        /// </summary>
        /// <returns>Question list</returns>
        List<SingleMultipleAnswerQuestion> GetAllQuestions();

        /// <summary>
        /// Add new code snippet question to the database
        /// </summary>
        /// <param name="codeSnippetQuestionModel">Code Snippet Question Model</param>
        /// <returns>
        /// returns 0 : When code snippet question is added successfully to the database
        /// returns 1 : When code snippet question failed to add 
        /// </returns>
        int AddCodeSnippetQuestion(CodeSnippetQuestion codeSnippetQuestion);
    }
}
