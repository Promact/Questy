﻿using System.Collections.Generic;
using System.Linq;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using AutoMapper;
using Promact.Trappist.DomainModel.ApplicationClasses.QuestionFetchingDto;
using AutoMapper.QueryableExtensions;
using Promact.Trappist.DomainModel.Models.Question;

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
        /// Get all questions
        /// </summary>
        /// <returns>Question list</returns>
        public ICollection<ApplicationClass> GetAllQuestions()
        {
            var questions = _dbContext.SingleMultipleAnswerQuestion.ProjectTo<ApplicationClass>().ToList();
            questions.AddRange(_dbContext.CodeSnippetQuestion.ProjectTo<ApplicationClass>().ToList());
            return questions;

        }
        /// <summary>
        /// Add single multiple answer question into model
        /// </summary>
        /// <param name="singleMultipleAnswerQuestion"></param>
        /// <param name="singleMultipleAnswerQuestionOption"></param>
        public void AddSingleMultipleAnswerQuestion(SingleMultipleAnswerQuestion singleMultipleAnswerQuestion, List<SingleMultipleAnswerQuestionOption> singleMultipleAnswerQuestionOption)
        {
            _dbContext.SingleMultipleAnswerQuestion.Add(singleMultipleAnswerQuestion);
            foreach (SingleMultipleAnswerQuestionOption singleMultipleAnswerQuestionOptionElement in singleMultipleAnswerQuestionOption)
            {
                singleMultipleAnswerQuestionOptionElement.SingleMultipleAnswerQuestionID = singleMultipleAnswerQuestion.Id;
                _dbContext.SingleMultipleAnswerQuestionOption.Add(singleMultipleAnswerQuestionOptionElement);
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Add new code snippet question to the database
        /// </summary>
        /// <param name="codeSnippetQuestion">Code Snippet Question Model</param>
        public void AddCodeSnippetQuestion(CodeSnippetQuestionDto codeSnippetQuestionModel)
        {

            CodeSnippetQuestion codeSnippetQuestion = Mapper.Map<CodeSnippetQuestionDto, CodeSnippetQuestion>(codeSnippetQuestionModel);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var question = _dbContext.CodeSnippetQuestion.Add(codeSnippetQuestion);
                _dbContext.SaveChanges();

                var codingLanguageList = codeSnippetQuestionModel.LanguageList;
                var questionId = question.Entity.Id;

                foreach (var language in codingLanguageList)
                {
                    var languageId = _dbContext.CodingLanguage.Where(x => x.Language == language).Select(x => x.Id).FirstOrDefault();
                    _dbContext.QuestionLanguageMapping.Add(new QuestionLanguageMapping
                    {
                        QuestionId = questionId,
                        LanguageId = languageId
                    });
                }
                _dbContext.SaveChanges();

                transaction.Commit();
            }
        }

    }
}
