using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Question;
using System.Linq;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Category;

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
		public List<Category> GetAllCategories()
        {
            return (_dbContext.Category.ToList());
        }
    }
}
