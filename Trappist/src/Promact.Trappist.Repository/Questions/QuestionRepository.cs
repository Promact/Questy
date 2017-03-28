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
        /// <param name="questionAC">Question data transfer object</param>
        public async Task AddCodeSnippetQuestionAsync(QuestionAC questionAC)
        {
            var codeSnippetQuestionModel = questionAC.CodeSnippetQuestionAC;
            CodeSnippetQuestion codeSnippetQuestion = Mapper.Map<CodeSnippetQuestionAC, CodeSnippetQuestion>(codeSnippetQuestionModel);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                //Add common question details
                var question = await _dbContext.Question.AddAsync(questionAC.Question);

                //Add codeSnippet part of question
                codeSnippetQuestion.QuestionId = question.Entity.Id;
                var codingQuestion = await _dbContext.CodeSnippetQuestion.AddAsync(codeSnippetQuestion);

                var languageIdList = await _dbContext.CodingLanguage.Select(x => x.Id).ToListAsync();

                //Map language to codeSnippetQuestion
                foreach (var languageId in languageIdList)
                {
                    await _dbContext.QuestionLanguageMapping.AddAsync(new QuestionLanguageMapping
                    {
                        QuestionId = codingQuestion.Entity.Id,
                        LanguageId = languageId
                    });
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}