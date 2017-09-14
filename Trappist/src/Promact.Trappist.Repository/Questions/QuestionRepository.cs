using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Questions
{
    public class QuestionRepository : IQuestionRepository
    {
        #region Private Member
        private readonly TrappistDbContext _dbContext;
        #endregion

        #region Constructor
        public QuestionRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Public Method
        public async Task<bool> IsQuestionExistAsync(int questionId)
        {
            return await _dbContext.Question.AnyAsync(x => x.Id == questionId);
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync(string userId, int id, int categoryId, string difficultyLevel, string searchQuestion)
        {
            var questionList = _dbContext.Question.Where(u => u.CreatedByUserId.Equals(userId));
            if (categoryId != 0)
            {
                questionList = questionList.Where(x => x.CategoryID == categoryId);
            }
            if (!difficultyLevel.Equals("All"))
            {
                var difficultyLevelCode = (DifficultyLevel)Enum.Parse(typeof(DifficultyLevel), difficultyLevel);
                questionList = questionList.Where(x => x.DifficultyLevel == difficultyLevelCode);
            }
            if (searchQuestion != null)
                questionList = questionList.Where(x => x.QuestionDetail.ToLowerInvariant().Contains(searchQuestion.ToLowerInvariant()));
            if (id == 0)
                return await questionList.Include(x => x.Category).Include(x => x.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).OrderByDescending(x => x.CreatedDateTime).Take(10).ToListAsync();
            else
                return await questionList.Include(x => x.Category).Include(x => x.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).Where(x => x.Id < id).OrderByDescending(x => x.CreatedDateTime).Take(10).ToListAsync();
        }

        public async Task<QuestionAC> AddSingleMultipleAnswerQuestionAsync(QuestionAC questionAC, string userId)
        {
            var question = Mapper.Map<QuestionDetailAC, Question>(questionAC.Question);
            var singleMultipleAnswerQuestion = Mapper.Map<SingleMultipleAnswerQuestionAC, SingleMultipleAnswerQuestion>(questionAC.SingleMultipleAnswerQuestion);
            question.CreatedByUserId = userId;

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                //Add common question details
                await _dbContext.Question.AddAsync(question);
                await _dbContext.SaveChangesAsync();

                //Add single/multiple question and option
                singleMultipleAnswerQuestion.Question = question;
                singleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption = questionAC.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption;
                await _dbContext.SingleMultipleAnswerQuestion.AddAsync(singleMultipleAnswerQuestion);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return (questionAC);
        }

        public async Task AddCodeSnippetQuestionAsync(QuestionAC questionAC, string userId)
        {
            var codeSnippetQuestion = Mapper.Map<CodeSnippetQuestionAC, CodeSnippetQuestion>(questionAC.CodeSnippetQuestion);
            var question = Mapper.Map<QuestionDetailAC, Question>(questionAC.Question);
            question.CreatedByUserId = userId;

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                //Add common question details
                await _dbContext.Question.AddAsync(question);
                await _dbContext.SaveChangesAsync();

                //Add codeSnippet part of question
                codeSnippetQuestion.Question = question;
                codeSnippetQuestion.CodeSnippetQuestionTestCases = questionAC.CodeSnippetQuestion.CodeSnippetQuestionTestCases;

                codeSnippetQuestion.CodeSnippetQuestionTestCases.ToList().ForEach(x =>
                {
                    x.TestCaseMarks = Math.Round(x.TestCaseMarks, 2);
                    x.CreatedDateTime = DateTime.UtcNow;
                });

                await _dbContext.CodeSnippetQuestion.AddAsync(codeSnippetQuestion);
                await _dbContext.SaveChangesAsync();
                var codingLanguages = await _dbContext.CodingLanguage.ToListAsync();

                //Map language to codeSnippetQuestion
                foreach (var language in questionAC.CodeSnippetQuestion.LanguageList)
                {
                    await _dbContext.QuestionLanguageMapping.AddAsync(new QuestionLanguageMapping
                    {
                        QuestionId = codeSnippetQuestion.Id,
                        LanguageId = codingLanguages.First(x => x.Language.ToLower().Equals(language.ToLower())).Id
                    });
                }
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task<ICollection<string>> GetAllCodingLanguagesAsync()
        {
            var codingLanguageList = await _dbContext.CodingLanguage.ToListAsync();

            var languageNameList = new List<string>();

            //Converting Enum value to string and adding it to languageNameList
            codingLanguageList.ForEach(codingLanguage =>
            {
                languageNameList.Add(codingLanguage.Language);
            });
            return languageNameList;
        }

        public async Task UpdateCodeSnippetQuestionAsync(QuestionAC questionAC, string userId)
        {
            var updatedQuestion = await _dbContext.Question.FindAsync(questionAC.Question.Id);

            await _dbContext.Entry(updatedQuestion).Reference(x => x.CodeSnippetQuestion).LoadAsync();
            await _dbContext.Entry(updatedQuestion.CodeSnippetQuestion).Collection(x => x.CodeSnippetQuestionTestCases).LoadAsync();

            var testCases = updatedQuestion.CodeSnippetQuestion.CodeSnippetQuestionTestCases;
            var updatedTestCase = questionAC.CodeSnippetQuestion.CodeSnippetQuestionTestCases;

            Mapper.Map(questionAC.Question, updatedQuestion);
            Mapper.Map(questionAC.CodeSnippetQuestion, updatedQuestion.CodeSnippetQuestion);
            updatedQuestion.UpdatedByUserId = userId;
            await _dbContext.SaveChangesAsync();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                //Handling one to many relationship with TestCase
                //Finding TestCase to be updated, deleted and added
                var testCaseToUpdate = updatedTestCase.Where(x => testCases.Any(y => y.Id == x.Id)).ToList();
                var testCaseToDelete = testCases.Where(x => !updatedTestCase.Any(y => y.Id == x.Id)).ToList();
                var testCaseToAdd = updatedTestCase.Where(x => !testCases.Any(y => y.Id == x.Id)).ToList();

                //Deleting TestCases
                _dbContext.CodeSnippetQuestionTestCases.RemoveRange(testCaseToDelete);
                await _dbContext.SaveChangesAsync();

                //Adding TestCases
                testCaseToAdd.ForEach(x =>
                {
                    x.CodeSnippetQuestionId = updatedQuestion.CodeSnippetQuestion.Id;
                    x.Id = 0;
                });
                await _dbContext.CodeSnippetQuestionTestCases.AddRangeAsync(testCaseToAdd);
                await _dbContext.SaveChangesAsync();

                //Updating TestCases
                testCaseToUpdate.ForEach(x =>
                {
                    var testCase = testCases.Single(test => test.Id == x.Id);
                    var testCaseEntry = _dbContext.Entry(testCase);
                    testCaseEntry.CurrentValues.SetValues(x);
                });
                await _dbContext.SaveChangesAsync();

                //Handling many to many relationship entity
                //Remove all the mapping between CodeSnippetQuestion and CodingLanguage
                var codingLanguageToRemove = await _dbContext.QuestionLanguageMapping.Where(x => x.QuestionId == updatedQuestion.CodeSnippetQuestion.Id).ToListAsync();
                _dbContext.QuestionLanguageMapping.RemoveRange(codingLanguageToRemove);

                var questionLanguageMapping = new List<QuestionLanguageMapping>();
                var languageList = await _dbContext.CodingLanguage.ToListAsync();

                //Map language to codeSnippetQuestion
                foreach (var language in questionAC.CodeSnippetQuestion.LanguageList)
                {
                    questionLanguageMapping.Add(new QuestionLanguageMapping
                    {
                        QuestionId = updatedQuestion.CodeSnippetQuestion.Id,
                        LanguageId = languageList.First(x => x.Language.ToLower().Equals(language.ToLower())).Id
                    });
                }
                updatedQuestion.CodeSnippetQuestion.QuestionLanguangeMapping = questionLanguageMapping;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task<QuestionAC> GetQuestionByIdAsync(int id)
        {
            var questionAC = new QuestionAC();

            var question = await _dbContext.Question.FindAsync(id);

            if (question == null)
                return null;
            if (question.QuestionType == QuestionType.Programming)
                await _dbContext.Entry(question).Reference(x => x.CodeSnippetQuestion).LoadAsync();
            else { 
                await _dbContext.Entry(question).Reference(x => x.SingleMultipleAnswerQuestion).LoadAsync();
                await _dbContext.Entry(question.SingleMultipleAnswerQuestion).Collection(x => x.SingleMultipleAnswerQuestionOption).LoadAsync();
            }

            questionAC.Question = Mapper.Map<Question, QuestionDetailAC>(question);
            questionAC.SingleMultipleAnswerQuestion = Mapper.Map<SingleMultipleAnswerQuestion, SingleMultipleAnswerQuestionAC>(question.SingleMultipleAnswerQuestion);
            questionAC.CodeSnippetQuestion = Mapper.Map<CodeSnippetQuestion, CodeSnippetQuestionAC>(question.CodeSnippetQuestion);

            if (question.QuestionType == QuestionType.Programming)
            {
                questionAC.CodeSnippetQuestion.LanguageList = await _dbContext
                    .QuestionLanguageMapping
                    .Where(x => x.QuestionId == question.Id)
                    .Include(x => x.CodeLanguage)
                    .Select(x => x.CodeLanguage.Language)
                    .ToListAsync();

                var testCases = await _dbContext
                    .CodeSnippetQuestionTestCases
                    .Where(x => x.CodeSnippetQuestionId == question.Id)
                    .ToListAsync();

                questionAC.CodeSnippetQuestion.CodeSnippetQuestionTestCases = testCases;
            }

            return questionAC;
        }

        public async Task UpdateSingleMultipleAnswerQuestionAsync(int questionId, QuestionAC questionAC, string userId)
        {
            var updatedQuestion = await _dbContext.Question.FindAsync(questionId);
            await _dbContext.Entry(updatedQuestion).Reference(x => x.SingleMultipleAnswerQuestion).LoadAsync();
            await _dbContext.Entry(updatedQuestion.SingleMultipleAnswerQuestion).Collection(x => x.SingleMultipleAnswerQuestionOption).LoadAsync();
            var singleMultipleQuestionAnswerOption = updatedQuestion.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption;
            var updatedOption = questionAC.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption;

            Mapper.Map(questionAC.Question, updatedQuestion);
            Mapper.Map(questionAC.SingleMultipleAnswerQuestion, updatedQuestion.SingleMultipleAnswerQuestion);
            updatedQuestion.SingleMultipleAnswerQuestion.UpdateDateTime = DateTime.UtcNow;
            updatedQuestion.UpdatedByUserId = userId;
            await _dbContext.SaveChangesAsync();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var optionToUpdate = updatedOption.Where(x => singleMultipleQuestionAnswerOption.Any(y => y.Id == x.Id)).ToList();
                var optionToDelete = singleMultipleQuestionAnswerOption.Where(x => !updatedOption.Any(y => y.Id == x.Id)).ToList();
                var optionToAdd = updatedOption.Where(x => !singleMultipleQuestionAnswerOption.Any(y => y.Id == x.Id)).ToList();

                //Remove options from updated question
                _dbContext.SingleMultipleAnswerQuestionOption.RemoveRange(optionToDelete);
                await _dbContext.SaveChangesAsync();

                //Add new options to updated question
                optionToAdd.ForEach(x =>
                {
                    x.SingleMultipleAnswerQuestionID = updatedQuestion.SingleMultipleAnswerQuestion.Id;
                    x.Id = 0;
                });
                await _dbContext.SingleMultipleAnswerQuestionOption.AddRangeAsync(optionToAdd);
                await _dbContext.SaveChangesAsync();

                //Update options details
                optionToUpdate.ForEach(x =>
                {
                    var singleMultipleAnswerQuestionOptionEntry = _dbContext.Entry<SingleMultipleAnswerQuestionOption>(singleMultipleQuestionAnswerOption.Single(test => test.Id == x.Id));
                    singleMultipleAnswerQuestionOptionEntry.CurrentValues.SetValues(x);
                });
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task<bool> IsQuestionExistInTestAsync(int id)
        {
            return await _dbContext.TestQuestion.AnyAsync(x => x.QuestionId == id);
        }

        public async Task DeleteQuestionAsync(int id)
        {
            var questionToDelete = await _dbContext.Question.FindAsync(id);
            _dbContext.Question.Remove(questionToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<QuestionCount> GetNumberOfQuestionsAsync(string userId, int categoryId, string matchString)
        {
            var questionCount = new QuestionCount();
            var questionList = _dbContext.Question.Where(u => u.CreatedByUserId.Equals(userId));
            if (categoryId != 0)
                questionList = questionList.Where(x => x.CategoryID == categoryId);
            if (matchString != null)
                questionList = questionList.Where(x => x.QuestionDetail.ToLowerInvariant().Contains(matchString.ToLowerInvariant()));
            await questionList.ForEachAsync(question =>
            {
                switch (question.DifficultyLevel)
                {
                    case DifficultyLevel.Easy:
                        questionCount.EasyCount++;
                        break;
                    case DifficultyLevel.Medium:
                        questionCount.MediumCount++;
                        break;
                    case DifficultyLevel.Hard:
                        questionCount.HardCount++;
                        break;
                }
            });
            return questionCount;
        }
        #endregion
    }
}