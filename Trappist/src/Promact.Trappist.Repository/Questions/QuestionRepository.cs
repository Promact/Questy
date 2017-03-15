﻿using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Question;
using System.Linq;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;

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
        public void AddCodeSnippetQuestion(CodeSnippetQuestionModel codeSnippetQuestion)
        {
            var question = _dbContext.CodeSnippetQuestion.Add(codeSnippetQuestion);
            _dbContext.SaveChanges();

            var codingLanguageList = codeSnippetQuestion.LanguageList;
            var questionId = question.Entity.Id;

            foreach(var language in codingLanguageList)
            {
                var languageId = _dbContext.CodingLanguage.Where(x => x.Language == language).Select(x => x.Id).FirstOrDefault();
                _dbContext.QuestionLanguageMapping.Add(new QuestionLanguageMapping
                {
                    QuestionId = questionId,
                    LanguageId = languageId
                });
                _dbContext.SaveChanges();                
            }



           
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
