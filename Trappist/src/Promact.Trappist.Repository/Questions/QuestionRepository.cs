using System.Collections.Generic;
using System.Linq;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.SingleMultipleAnswerQuestionApplicationClass;
using System.Threading.Tasks;

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
        public ICollection<SingleMultipleAnswerQuestionApplicationClass> GetAllQuestions()
        {
            var questions = _dbContext.SingleMultipleAnswerQuestion.ProjectTo<SingleMultipleAnswerQuestionApplicationClass>().ToList();
            questions.AddRange(_dbContext.CodeSnippetQuestion.ProjectTo<SingleMultipleAnswerQuestionApplicationClass>().ToList());
            var questionsOrderedByCreatedDateTime = questions.OrderBy(f => f.CreatedDateTime).ToList();
            return questionsOrderedByCreatedDateTime;
        }
        /// <summary>
        /// Add single multiple answer question into model
        /// </summary>
        /// <param name="singleMultipleAnswerQuestion"></param>
        /// <param name="singleMultipleAnswerQuestionOption"></param>
        public async Task<QuestionAC>  AddSingleMultipleAnswerQuestionAsync(QuestionAC questionAC)
        {
            await _dbContext.SingleMultipleAnswerQuestion.AddAsync(questionAC.SingleMultipleQuestion.SingleMultipleAnswerQuestion);
            foreach (SingleMultipleAnswerQuestionOption singleMultipleAnswerQuestionOptionElement in questionAC.SingleMultipleQuestion.SingleMultipleAnswerQuestionOption)
            {
                //To-Do Change according to new model singleMultipleAnswerQuestionOptionElement.SingleMultipleAnswerQuestionID = singleMultipleAnswerQuestion.Id;
                await _dbContext.SingleMultipleAnswerQuestionOption.AddAsync(singleMultipleAnswerQuestionOptionElement);
            }
            _dbContext.SaveChanges();
            return(questionAC);
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
                //To-Do Change according to new model var questionId = question.Entity.Id; 
                foreach (var language in codingLanguageList)
                {
                    var languageId = _dbContext.CodingLanguage.Where(x => x.Language == language).Select(x => x.Id).FirstOrDefault();
                    _dbContext.QuestionLanguageMapping.Add(new QuestionLanguageMapping
                    {
                        //To-Do Change according to new model QuestionId = questionId,
                        LanguageId = languageId
                    });
                }
                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }
    }
}