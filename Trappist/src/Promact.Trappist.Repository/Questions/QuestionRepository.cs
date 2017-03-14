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
        /// <returns>
        /// returns 0 : When code snippet question is added successfully to the database
        /// returns 1 : When code snippet question failed to add 
        /// </returns>
        public async Task<int> AddCodeSnippetQuestion(CodeSnippetQuestion codeSnippetQuestion)
        {
            try
            {
                _dbContext.CodeSnippetQuestion.Add(codeSnippetQuestion);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return 0;
            }
            return 1;
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
