using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Question;
using System.Linq;
using Promact.Trappist.DomainModel.DbContext;

namespace Promact.Trappist.Repository.Questions
{
    public class QuestionsRepository : IQuestionsRespository
    {
        private readonly TrappistDbContext _dbContext;

        public QuestionsRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get all questions
        /// </summary>
        /// <returns>Question list</returns>
        public List<SingleMultipleAnswerQuestion> GetAllQuestions()
        {
            var questions = _dbContext.SingleMultipleAnswerQuestion.ToList();
            
            return questions;
        }
    }
}
