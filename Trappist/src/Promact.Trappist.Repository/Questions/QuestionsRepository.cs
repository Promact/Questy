using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Web.Models;
using System.Linq;
using Promact.Trappist.Web.Data;

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
        public List<Question> GetAllQuestions()
        {
            var questions = _dbContext.Question.ToList();
            
            return questions;
        }
    }
}
