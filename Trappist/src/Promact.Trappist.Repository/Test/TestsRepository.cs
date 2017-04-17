using Promact.Trappist.DomainModel.DbContext;
using System.Linq;
using Promact.Trappist.DomainModel.Models.Test;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.Utility.GlobalUtil;
using System;
using AutoMapper;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.Utility.ExtensionMethods;
using System.Globalization;

namespace Promact.Trappist.Repository.Tests
{
    public class TestsRepository : ITestsRepository
    {
        private readonly TrappistDbContext _dbContext;
        private readonly IGlobalUtil _util;
        private List<TestQuestion> testQuestionList;
        private QuestionAC questionAc;
        private List<QuestionAC> questionListAC;
        private TestAC testACObject;
        private List<CategoryAC> categoryListAC;
        private TestQuestion testQuestionObj;
        private List<Category> categoryList;
        private bool isDeleted = false;


        public TestsRepository(TrappistDbContext dbContext, IGlobalUtil util)
        {
            _dbContext = dbContext;
            _util = util;
            testACObject = new TestAC();
            testQuestionList = new List<TestQuestion>();         
            questionListAC = new List<QuestionAC>();
            categoryListAC = new List<CategoryAC>();
            categoryList = new List<Category>();
        }
        #region Test
        /// <summary>
        /// this method is used to create a new test
        /// </summary>
        /// <param name="test">object of Test</param>
        public async Task CreateTestAsync(Test test)
        {
            test.TestName = test.TestName.AllTrim();
            test.Link = _util.GenerateRandomString(10);
            _dbContext.Test.Add(test);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsTestNameUniqueAsync(string testName, int id)
        {
            var isTestExists = await (_dbContext.Test.AnyAsync(x =>
                                    x.TestName.ToLowerInvariant() == testName.AllTrim().ToLowerInvariant()
                                    && x.Id != id));
            return !isTestExists;
        }

        public async Task<List<Test>> GetAllTestsAsync()
        {
            return await _dbContext.Test.OrderByDescending(x => x.CreatedDateTime).ToListAsync();
        }
        #endregion
        #region Test Settings
        /// <summary>
        /// Updates the edited Test Name
        /// </summary>
        /// <param name="id">The parameter "id" takes takes the value of the Id from the route</param>
        /// <param name="testObject">The parameter "testObject" is an object of Test</param>
        /// <returns>Updated Test Name</returns>
        public async Task UpdateTestNameAsync(int id, Test testObject)
        {
            var testSettingsToUpdate = _dbContext.Test.FirstOrDefault(x => x.Id == id);
            testObject.TestName = testObject.TestName.AllTrim();
            testSettingsToUpdate.TestName = testObject.TestName;
            _dbContext.Test.Update(testSettingsToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTestByIdAsync(Test testObject)
        {
            testObject.TestName = testObject.TestName.AllTrim();
            _dbContext.Test.Update(testObject);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Test> GetTestByIdAsync(int id)
        {
            string currentDate = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime date = DateTime.ParseExact(currentDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            var testSettings = await _dbContext.Test.FirstOrDefaultAsync(x => x.Id == id);
            if (testSettings != null)
            {
                testSettings.StartDate = testSettings.StartDate == default(DateTime) ? date : testSettings.StartDate; //If the StartDate field in database contains default value on visiting the Test Settings page of a Test for the first time then that default value gets replaced by current DateTime
                testSettings.EndDate = testSettings.EndDate == default(DateTime) ? date : testSettings.EndDate; //If the EndDate field in database contains default value on visiting the Test Settings page of a Test for the first time then that default value gets replaced by current DateTime
                return testSettings;
            }
            else
                return null;
        }

        public async Task<bool> IsTestExists(int id)
        {
            return await _dbContext.Test.AnyAsync(x => x.Id == id);
        }
        #endregion
        #region Delete Test
        public async Task<bool> IsTestAttendeeExistAsync(int id)
        {
            Test test = await _dbContext.Test.Include(x => x.TestAttendees).FirstOrDefaultAsync(x => x.Id == id);
            return test.TestAttendees.Any();
        }

        public async Task DeleteTestAsync(int id)
        {
            Test test = await _dbContext.Test.FindAsync(id);
            _dbContext.Test.Remove(test);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<QuestionAC>> GetAllTestCategoryQuestionsByIdAsync(int testId, int categoryId)
        {
            var testQuestionList = await _dbContext.TestQuestion.Where(x=>x.TestId == testId).ToListAsync();
            var questionList = await _dbContext.Question.Where(x => x.CategoryID == categoryId).Include(y => y.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).ToListAsync();
                     
                foreach (var question in questionList)
                {
                    questionAc = new QuestionAC();
                    questionAc.Question = Mapper.Map<Question, QuestionDetailAC>(question);
                    questionAc.SingleMultipleAnswerQuestion = Mapper.Map<SingleMultipleAnswerQuestion, SingleMultipleAnswerQuestionAC>(question.SingleMultipleAnswerQuestion);
                    if (testQuestionList.Exists(x=> x.QuestionId == questionAc.Question.Id && x.TestId == testId))
                        questionAc.Question.IsSelect = true;
                questionListAC.Add(questionAc);
                }
            return questionListAC;
        }

        public async Task<string> AddTestQuestionsAsync(List<QuestionAC> QuestionsToAddTest,int testId)
        {        
            foreach (var questionToAdd in QuestionsToAddTest)
            {
                var question = Mapper.Map<QuestionDetailAC, Question>(questionToAdd.Question);
                var TestCategory = _dbContext.TestCategory.FirstOrDefault(x=>x.CategoryId==question.CategoryID);
                var QuestionExistInTest = await _dbContext.TestQuestion.FirstOrDefaultAsync(x => x.QuestionId == question.Id);
                if (QuestionExistInTest != null && !questionToAdd.Question.IsSelect)
                {
                    _dbContext.TestQuestion.Remove(QuestionExistInTest);
                    await _dbContext.SaveChangesAsync();
                    isDeleted = true;
                }
                    
                else if (questionToAdd.Question.IsSelect)
                {
                    testQuestionObj = new TestQuestion();
                    testQuestionObj.QuestionId = question.Id;
                    testQuestionObj.TestCategoryId = TestCategory.Id;
                    testQuestionObj.TestId = testId;
                    if (!await _dbContext.TestQuestion.AnyAsync(x =>  x.QuestionId == question.Id && x.TestId == testId))
                        testQuestionList.Add(testQuestionObj);
                }                 
            }
            if (testQuestionList.Count == 0 && !isDeleted)
                return "No new questions selected..";
            else
            {
                
                    await _dbContext.TestQuestion.AddRangeAsync(testQuestionList);
                    await _dbContext.SaveChangesAsync();
                    return "Your changes saved successfully";  
            }                
        }

        public async Task<TestAC> GetTestDetailsByIdAsync(int testId)
        {          
            var test = await _dbContext.Test.FirstOrDefaultAsync(x=>x.Id == testId);
            testACObject = Mapper.Map<Test, TestAC>(test);         
            var testCategoryList =   await _dbContext.TestCategory.Where(x=>x.TestId== testId).Include(x=>x.Category).ToListAsync();
            foreach (var testcategory in testCategoryList)
            {
                categoryList.Add(testcategory.Category);
            }
            categoryListAC = Mapper.Map<List<Category>, List<CategoryAC>>(categoryList);
            testACObject.CategoryACList = categoryListAC;
            return testACObject;          
        }
        #endregion
    }
}