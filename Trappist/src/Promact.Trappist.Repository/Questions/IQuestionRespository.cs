using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.SingleMultipleAnswerQuestionApplicationClass;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Questions
{
    public interface IQuestionRespository
    {
        /// <summary>
        /// Add single multiple answer question into model
        /// </summary>
        /// <param name="singleMultipleAnswerQuestion"></param>
        /// <param name="singleMultipleAnswerQuestionOption"></param>
        Task<QuestionAC> AddSingleMultipleAnswerQuestionAsync(QuestionAC questionAC);
        /// <summary>
        /// Add new code snippet question to the database
        /// </summary>
        /// <param name="codeSnippetQuestionModel">Code Snippet Question Model</param>
        void AddCodeSnippetQuestion(CodeSnippetQuestionDto codeSnippetQuestionModel);
        /// <summary>
        /// Get all questions
        /// </summary>
        /// <returns>Question list</returns>
        ICollection<SingleMultipleAnswerQuestionApplicationClass> GetAllQuestions();
    }
}