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
        List<SingleMultipleAnswerQuestion> GetAllQuestions();
        void AddSingleMultipleAnswerQuestion(SingleMultipleAnswerQuestion singleMultipleAnswerQuestion);
        void AddSingleMultipleAnswerQuestionOption(SingleMultipleAnswerQuestionOption singleMultipleAnswerQuestionOption);
    }
    
}
