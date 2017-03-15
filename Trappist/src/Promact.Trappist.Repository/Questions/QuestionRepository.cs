using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Question;
using System.Linq;
using Promact.Trappist.DomainModel.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        /// Add new code snippet question to the database
        /// </summary>
        /// <param name="codeSnippetQuestion">Code Snippet Question Model</param>
        public void AddCodeSnippetQuestion(CodeSnippetQuestion codeSnippetQuestion)
        {
           _dbContext.CodeSnippetQuestion.Add(codeSnippetQuestion);
           _dbContext.SaveChanges();
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
