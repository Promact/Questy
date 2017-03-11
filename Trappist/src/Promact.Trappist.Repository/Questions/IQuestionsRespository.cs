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
        IEnumerable<string> GetAllCategories();
        IEnumerable<SingleMultipleAnswerQuestion> GetAllQuestions();
    }
}
