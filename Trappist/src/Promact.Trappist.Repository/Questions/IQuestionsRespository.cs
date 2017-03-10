using Promact.Trappist.DomainModel.Models.Category;
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
        List<Question> GetAllQuestions();
        List<Category> GetAllCategories();
        List<SingleMultipleAnswerQuestion> GetAllQuestions();
    }
}
