using Promact.Trappist.DomainModel.ApplicationClasses.Question;
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
        /// <param name="questionAC">Question data transfer object</param>
        Task AddCodeSnippetQuestionAsync(QuestionAC questionAC);

        /// <summary>
        /// Get all Questions
        /// </summary>
        /// <returns>Question list</returns>
       Task <ICollection<Question>> GetAllQuestionsAsync();
    }
}