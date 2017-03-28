using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Promact.Trappist.Repository.Questions
{
    public interface IQuestionRespository
    {
        /// <summary>
        /// A method to add single multiple answer question.
        /// </summary>
        /// <param name="questionAC">Object of QuestionAC</param>
        /// <param name="userEmail">Email id of user</param>
        /// <returns>Return object of QuestionAC</returns>
        Task<QuestionAC> AddSingleMultipleAnswerQuestionAsync(QuestionAC questionAC, string userEmail);

        /// <summary>
        /// Add new code snippet question to the database
        /// </summary>
        /// <param name="questionAC">Question data transfer object</param>
        Task AddCodeSnippetQuestionAsync(QuestionAC questionAC);

        /// <summary>
        /// Method to get all the Questions
        /// </summary>
        /// <returns>Question list</returns>
        Task <ICollection<Question>> GetAllQuestionsAsync();

        /// <summary>
        /// Gets all the coding languages as string from database
        /// </summary>
        /// <returns></returns>
        Task<ICollection<CodingLanguageAC>> GetAllCodingLanguagesAsync();
    }
}