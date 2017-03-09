using Promact.Trappist.DomainModel.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Questions
{
    public interface IQuestionsRespository
    {
        /// <summary>
        /// Get all questions
        /// </summary>
        /// <returns>Question list</returns>
        List<Question> GetAllQuestions();

        /// <summary>
        /// Adds code snippet question to the CodeSnippetQuestion table
        /// </summary>
        /// <param name="question">Code snippet Question</param>
        /// <returns>
        /// returns 0: Add operation failed
        /// returns 1: Add operation succeeded
        /// </returns>
        int AddCodeSnippetQuestion(CodeSnippetQuestion question);
    }
}
