using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.ApplicationClasses;
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
        ///Method to get all the Questions
        /// </summary>
        /// <returns>Question list</returns>
        public async Task<ICollection<Question>> GetAllQuestionsAsync()
        {
            return (await _dbContext.Question.Include(x => x.Category).Include(x => x.CodeSnippetQuestion).Include(x => x.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).OrderByDescending(g => g.CreatedDateTime).ToListAsync());
        }

        /// <summary>
        /// A method to add single multiple answer question.
        /// </summary>
        /// <param name="questionAC">Object of QuestionAC</param>
        /// <returns>Returns object of QuestionAC</returns>
        public async Task<QuestionAC>  AddSingleMultipleAnswerQuestionAsync(QuestionAC questionAC)
        {
            var question = Mapper.Map<QuestionDetailAC, Question>(questionAC.Question);
            var singleMultipleQuestion = Mapper.Map<SingleMultipleAnswerQuestionAC,SingleMultipleAnswerQuestion>(questionAC.SingleMultipleAnswerQuestion);
            question.CreatedBy = "Admin";

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                //Add common question details
                await _dbContext.Question.AddAsync(question);
                await _dbContext.SaveChangesAsync();

                //Add single/multiple question and option
                singleMultipleQuestion.Id = question.Id;
                await _dbContext.SingleMultipleAnswerQuestion.AddAsync(singleMultipleQuestion);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }            
            return(questionAC);
        }

        /// <summary>
        /// Add new code snippet question to the database
        /// </summary>
        /// <param name="questionAC">Question data transfer object</param>
        public async Task AddCodeSnippetQuestionAsync(QuestionAC questionAC)
        {
            var codeSnippetQuestion =questionAC.CodeSnippetQuestion;
            var question = Mapper.Map<QuestionDetailAC, Question>(questionAC.Question);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                //Add common question details
                await _dbContext.Question.AddAsync(question);
                await _dbContext.SaveChangesAsync();

                //Add codeSnippet part of question
                codeSnippetQuestion.Id = question.Id;
                await _dbContext.CodeSnippetQuestion.AddAsync(questionAC.CodeSnippetQuestion);
                await _dbContext.SaveChangesAsync();

                var languageIdList = await _dbContext.CodingLanguage.Select(x => x.Id).ToListAsync();

                //Map language to codeSnippetQuestion
                foreach (var languageId in languageIdList)
                {
                    await _dbContext.QuestionLanguageMapping.AddAsync(new QuestionLanguageMapping
                    {
                        QuestionId = codeSnippetQuestion.Id,
                        LanguageId = languageId
                    });
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}