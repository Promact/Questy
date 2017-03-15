﻿using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Question;
using System.Linq;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;

namespace Promact.Trappist.Repository.Questions
{
    public class QuestionRepository : IQuestionRespository
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
            var question = new CodeSnippetQuestion
            {
                CategoryID = codeSnippetQuestion.CategoryId,
                QuestionDetail = codeSnippetQuestion.QuestionDetail,
                CreateBy = codeSnippetQuestion.CreateBy,
                QuestionType = codeSnippetQuestion.QuestionType,
                DifficultyLevel = codeSnippetQuestion.DifficultyLevel,
                CheckCodeComplexity = codeSnippetQuestion.CheckCodeComplexity,
                RunBasicTestCase = codeSnippetQuestion.RunBasicTestCase,
                RunCornerTestCase = codeSnippetQuestion.RunCornerTestCase,
                RunNecessaryTestCase = codeSnippetQuestion.RunNecessaryTestCase,
                CheckTimeComplexity = codeSnippetQuestion.CheckTimeComplexity,
                CreatedDateTime = codeSnippetQuestion.CreatedDateTime

            };

           _dbContext.CodeSnippetQuestion.Add(codeSnippetQuestion);
           _dbContext.SaveChanges();
        }

        /// <summary>
        /// Get all questions
        /// </summary>
        /// <returns>Question list</returns>
        public List<SingleMultipleAnswerQuestion> GetAllQuestions()
        {
            var question = _dbContext.SingleMultipleAnswerQuestion.ToList();
            return question;
        }
        /// <summary>
        /// Add single multiple answer question into model
        /// </summary>
        /// <param name="singleMultipleAnswerQuestion"></param>
        /// <param name="singleMultipleAnswerQuestionOption"></param>
        public void AddSingleMultipleAnswerQuestion(SingleMultipleAnswerQuestion singleMultipleAnswerQuestion, List<SingleMultipleAnswerQuestionOption> singleMultipleAnswerQuestionOption)
        {
            _dbContext.SingleMultipleAnswerQuestion.Add(singleMultipleAnswerQuestion);
            foreach(SingleMultipleAnswerQuestionOption singleMultipleAnswerQuestionOptionElement in singleMultipleAnswerQuestionOption)
            {
                singleMultipleAnswerQuestionOptionElement.SingleMultipleAnswerQuestionID = singleMultipleAnswerQuestion.Id;
                _dbContext.SingleMultipleAnswerQuestionOption.Add(singleMultipleAnswerQuestionOptionElement);
            }   
            _dbContext.SaveChanges();
        }
   
   
    }
}
