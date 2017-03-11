using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Question;
using System.Linq;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Category;
using Microsoft.EntityFrameworkCore;

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
        /// Get all the names of Categories
        /// </summary>
        /// <returns>Categories list</returns>
        public IEnumerable<string> GetAllCategories()
        {
            return (_dbContext.Category.Select(x=>x.CategoryName).ToList());
        }
        /// <summary>
        /// Get all questions
        /// </summary>
        /// <returns>Question list</returns>
        public IEnumerable<SingleMultipleAnswerQuestion> GetAllQuestions()
        {
           var questions = _dbContext.SingleMultipleAnswerQuestion.ToList();
            return questions;
        }

        
    }
}
