using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;
using System.Linq;
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
        ///Get All Questions
        /// </summary>
        /// <returns>Question list</returns>
        /// The function name ends with Async
        public async Task<ICollection<Question>> GetAllQuestionsAsync()
        {
            return (await _dbContext.Question.Include(x => x.Category).Include(x => x.CodeSnippetQuestion).Include(x => x.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).OrderByDescending(g => g.CreatedDateTime).ToListAsync());
        }

        /// <summary>
        /// A method to add single multiple answer question.
        /// </summary>
        /// <param name="singleMultipleAnswerQuestion"></param>
        /// <param name="singleMultipleAnswerQuestionOption"></param>
        public async Task<QuestionAC>  AddSingleMultipleAnswerQuestionAsync(QuestionAC questionAC)
        {
            await _dbContext.SingleMultipleAnswerQuestion.AddAsync(questionAC.SingleMultipleAnswerQuestionAC.SingleMultipleAnswerQuestion);
            foreach (SingleMultipleAnswerQuestionOption singleMultipleAnswerQuestionOptionElement in questionAC.SingleMultipleAnswerQuestionAC.SingleMultipleAnswerQuestionOption)
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