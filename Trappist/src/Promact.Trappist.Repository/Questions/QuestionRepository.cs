using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Promact.Trappist.Repository.Questions
{
    public class QuestionRepository : IQuestionRespository
    {
        private readonly TrappistDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionRepository(TrappistDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        /// <summary>
        /// Method to get all the Questions
        /// </summary>
        /// <returns>Question list</returns>
        public async Task<ICollection<Question>> GetAllQuestionsAsync()
        {
            return (await _dbContext.Question.Include(x => x.Category).Include(x => x.CodeSnippetQuestion).Include(x => x.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).OrderByDescending(g => g.CreatedDateTime).ToListAsync());
        }

        /// <summary>
        /// A method to add single multiple answer Question.
        /// </summary>
        /// <param name="questionAC">Object of QuestionAC</param>
        /// <param name="userEmail">Email id of user</param>
        /// <returns>Returns object of QuestionAC</returns>
        public async Task<QuestionAC> AddSingleMultipleAnswerQuestionAsync(QuestionAC questionAC, string userEmail)
        {
            var question = Mapper.Map<QuestionDetailAC, Question>(questionAC.Question);
            var singleMultipleAnswerQuestion = Mapper.Map<SingleMultipleAnswerQuestionAC, SingleMultipleAnswerQuestion>(questionAC.SingleMultipleAnswerQuestion);
            question.ApplicationUser = await _userManager.FindByEmailAsync(userEmail);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                //Add common question details
                await _dbContext.Question.AddAsync(question);
                await _dbContext.SaveChangesAsync();

                //Add single/multiple question and option
                singleMultipleAnswerQuestion.Question = question;
                await _dbContext.SingleMultipleAnswerQuestion.AddAsync(singleMultipleAnswerQuestion);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return (questionAC);
        }

        /// <summary>
        /// Add new code snippet Question to the database
        /// </summary>
        /// <param name="questionAC">Question data transfer object</param>
        public async Task AddCodeSnippetQuestionAsync(QuestionAC questionAC)
        {
            var codeSnippetQuestion = questionAC.CodeSnippetQuestion;
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

        /// <summary>
        /// Gets all the coding languages int the database
        /// </summary>
        /// <returns>coding language in CodingLanguageAC</returns>
        public async Task<ICollection<CodingLanguageAC>> GetAllCodingLanguageAsync()
        {
            var codingLanguage = await _dbContext.CodingLanguage.ToListAsync();

            ICollection<CodingLanguageAC> codingLanguageAC = new List<CodingLanguageAC>();
            codingLanguage.ForEach(x =>
            {
                codingLanguageAC.Add(new CodingLanguageAC
                {
                    LanguageCode = x.Language,
                    LanguageName = (x.Language).ToString()
                });
            });
            return codingLanguageAC;
        }
    }
}