using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Promact.Trappist.Repository.Questions
{
    public interface IQuestionRepository
    {
        /// <summary>
        /// A method to add single multiple answer question.
        /// </summary>
        /// <param name="questionAC">Object of QuestionAC</param>
        /// <param name="userEmail">Email id of user</param>
        /// <returns>Return object of QuestionAC</returns>
        Task<QuestionAC> AddSingleMultipleAnswerQuestionAsync(QuestionAC questionAC, string userEmail);

        /// <summary>
        /// Adds new code snippet question to the database
        /// </summary>
        /// <param name="questionAC">Question data transfer object</param>
        /// <param name="userEmail">Email of current user</param>
        Task AddCodeSnippetQuestionAsync(QuestionAC questionAC, string userEmail);

        /// <summary>
        /// Method to get all the Questions
        /// </summary>
        /// <returns>Question list</returns>
        Task <ICollection<Question>> GetAllQuestionsAsync();

        /// <summary>
        /// Gets all the coding languages as string from database
        /// </summary>
        /// <returns></returns>
        Task<ICollection<string>> GetAllCodingLanguagesAsync();
    }
}