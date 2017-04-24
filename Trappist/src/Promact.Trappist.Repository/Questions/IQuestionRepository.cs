using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Promact.Trappist.Repository.Questions
{
    public interface IQuestionRepository
    {
        /// <summary>
        /// A method to add single multiple answer question.
        /// </summary>
        /// <param name="questionAC">Object of QuestionAC</param>
        /// <param name="userId">Id of logged in user</param>
        /// <returns>Return object of QuestionAC</returns>
        Task<QuestionAC> AddSingleMultipleAnswerQuestionAsync(QuestionAC questionAC, string userId);

        /// <summary>
        /// Adds new code snippet question to the Database
        /// </summary>
        /// <param name="questionAC">QuestionAC class object</param>
        /// <param name="userId">Id of logged in user</param>
        Task AddCodeSnippetQuestionAsync(QuestionAC questionAC, string userId);

        /// <summary>
        /// Method to get all Questions
        /// </summary>
        /// <param name="userId">Id of logged in user</param>
        /// <returns></returns>
        Task<ICollection<Question>> GetAllQuestionsAsync(string userId);

        /// <summary>
        /// Gets all the coding languages as string from Database
        /// </summary>
        /// <returns>CodingLanguageAC class object</returns>
        Task<ICollection<string>> GetAllCodingLanguagesAsync();

        /// <summary>  
        /// Updates existing single multiple answer Question in the database  
        /// </summary>  
        /// <param name="questionId">Id of Question to update</param>  
        /// <param name="questionAC">QuestionAC class object</param>  
        /// <param name="userId">Id of logged in user</param>  
        Task UpdateSingleMultipleAnswerQuestionAsync(int questionId, QuestionAC questionAC, string userId);

        /// <summary>
        /// Method to check Question is exists or not.
        /// </summary>
        /// <param name="questionId">Id of Question</param>
        /// <returns>Return true if id already exists otherwise return false</returns>
        Task<bool> IsQuestionExistAsync(int questionId);

        /// <summary>
        ///  Updates existing code snippet question in the Database
        /// </summary>
        /// <param name="questionAC">QuestionAC class object</param>
        /// <param name="userId">Id of logged in user</param>
        Task UpdateCodeSnippetQuestionAsync(QuestionAC questionAC, string userId);

        /// <summary>
        /// Method to check Question exist in test or not
        /// </summary>
        /// <param name="id">Id to check Question</param>
        /// <returns>True if exist else false</returns>
        Task<bool> IsQuestionExistInTestAsync(int id);

        /// <summary>
        /// Method to delete Question
        /// </summary>
        /// <param name="question">Question object</param>
        Task DeleteQuestionAsync(int id);

        /// <summary>
        /// Method to get question
        /// </summary>
        /// <param name="id">Id of Question</param>
        /// <returns>Object of QuestionAC</returns>
        Task<QuestionAC> GetQuestionByIdAsync(int id);
    }
}