using System.Collections.Generic;
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
        public void AddCodeSnippetQuestion(CodeSnippetQuestionModel codeSnippetQuestionModel)
        {
            #region mapping
            CodeSnippetQuestion codeSnippetQuestion = new CodeSnippetQuestion
            {
                CategoryID = codeSnippetQuestionModel.CategoryID,
                QuestionDetail = codeSnippetQuestionModel.QuestionDetail,
                DifficultyLevel = codeSnippetQuestionModel.DifficultyLevel,
                QuestionType = codeSnippetQuestionModel.QuestionType,
                CheckCodeComplexity = codeSnippetQuestionModel.CheckCodeComplexity,
                CheckTimeComplexity = codeSnippetQuestionModel.CheckTimeComplexity,
                RunBasicTestCase = codeSnippetQuestionModel.RunBasicTestCase,
                RunCornerTestCase = codeSnippetQuestionModel.RunCornerTestCase,
                RunNecessaryTestCase = codeSnippetQuestionModel.RunNecessaryTestCase,
                CreateBy = codeSnippetQuestionModel.CreateBy,
                UpdatedBy = codeSnippetQuestionModel.UpdatedBy
                
            };
            #endregion

            var question = _dbContext.CodeSnippetQuestion.Add(codeSnippetQuestion);
            _dbContext.SaveChanges();

            var codingLanguageList = codeSnippetQuestionModel.LanguageList;
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
