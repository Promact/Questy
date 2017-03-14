using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;

namespace Promact.Trappist.Repository.Questions
{
    public interface IQuestionsRepository
    {
        /// <summary>
        /// Get all questions
        /// </summary>
        /// <returns>Question list</returns>
        List<SingleMultipleAnswerQuestion> GetAllQuestions();
    }
}
