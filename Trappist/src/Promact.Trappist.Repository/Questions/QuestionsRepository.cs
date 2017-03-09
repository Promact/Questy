using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Web.Data;
using System.Linq;
using System;

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
        /// Yet to be implemented
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public int AddCodeSnippetQuestion(CodeSnippetQuestion question)
        {
            _dbContext.CodeSnippetQuestion.Add(question);
            return 0;
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
