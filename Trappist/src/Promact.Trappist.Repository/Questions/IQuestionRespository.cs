using Promact.Trappist.DomainModel.ApplicationClasses.QuestionFetchingDto;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;

namespace Promact.Trappist.Repository.Questions
{
    public interface IQuestionRespository
    {
        /// <summary>
        /// Add single multiple answer question into model
        /// </summary>
        /// <param name="singleMultipleAnswerQuestion"></param>
        /// <param name="singleMultipleAnswerQuestionOption"></param>
        void AddSingleMultipleAnswerQuestion(SingleMultipleAnswerQuestion singleMultipleAnswerQuestion, List<SingleMultipleAnswerQuestionOption> singleMultipleAnswerQuestionOption);

        /// <summary>
        /// Get all questions
        /// </summary>
        /// <returns>Question list</returns>
        ICollection<QuestionFetchingDto> GetAllQuestions();
    }

}
