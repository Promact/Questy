using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Question;
using System.Linq;
using Promact.Trappist.DomainModel.DbContext;

namespace Promact.Trappist.Repository.Questions
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly TrappistDbContext _dbContext;

        public QuestionRepository(TrappistDbContext dbContext)
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
        /// <summary>
        /// Adding single multiple answer question into SingleMultipleAnswerQuestion model
        /// </summary>
        /// <param name="singleMultipleAnswerQuestion"></param>
        public void AddSingleMultipleAnswerQuestion(SingleMultipleAnswerQuestion singleMultipleAnswerQuestion)
        {
            _dbContext.SingleMultipleAnswerQuestion.Add(singleMultipleAnswerQuestion);
            _dbContext.SaveChanges();
        }


    }
}
