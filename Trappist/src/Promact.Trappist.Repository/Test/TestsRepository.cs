using Promact.Trappist.DomainModel.DbContext;
using System.Linq;
using Promact.Trappist.DomainModel.Models.Test;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.Utility.GlobalUtil;
using System;
using Promact.Trappist.Utility.ExtensionMethods;
using System.Globalization;
using AutoMapper;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Models.Category;

namespace Promact.Trappist.Repository.Tests
{
    public class TestsRepository : ITestsRepository
    {
        TestAC testAc = new TestAC();
        private readonly TrappistDbContext _dbContext;
        private readonly IGlobalUtil _util;
        public TestsRepository(TrappistDbContext dbContext, IGlobalUtil util)
        {
            _dbContext = dbContext;
            _util = util;
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
        #endregion
        #region Category selection
        public async Task<TestAC> GetTestDetailsByIdAsync(int testId)
        {
            var testAcObject = new TestAC();
            var test = await _dbContext.Test.FindAsync(testId);
            testAcObject = Mapper.Map<Test, TestAC>(test);
            var categoryList = await _dbContext.Category.ToListAsync();
            var categoryListAc = Mapper.Map<List<Category>, List<CategoryAC>>(categoryList);
            var testCategoryList = await _dbContext.TestCategory.Where(x => x.TestId == testId).Include(x => x.Category).ToListAsync();
            categoryListAc.ForEach(category =>
            {
                if (testCategoryList.Exists(x => x.CategoryId == category.Id))
                    category.IsSelect = true;
            });
            testAcObject.CategoryAcList = categoryListAc;
            return testAcObject;
        }
        public async Task AddSelectedCategoryAsync(List<TestCategory> testCategory)
        {
            List<TestCategory> testCategoryList = new List<TestCategory>();
            var testCategories = await _dbContext.TestCategory.ToListAsync();
            foreach (var category in testCategory)
            {
                if (!testCategories.Exists(x => x.CategoryId == category.CategoryId && x.TestId == category.TestId))
                    testCategoryList.Add(category);
            }
            await _dbContext.TestCategory.AddRangeAsync(testCategoryList);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeselectCategoryAync(int categoryId, int testId)
        {
            var questions = await _dbContext.Question.OrderBy(x => x.CategoryID).ToListAsync();
            var testQuestions = await _dbContext.TestQuestion.OrderBy(x => x.QuestionId).ToListAsync();
            foreach (var question in questions)
            {              
                    if (question.CategoryID == categoryId)
                    if(testQuestions.Exists(testQuestion=> testQuestion.QuestionId == question.Id && testQuestion.TestId == testId))
                        return true;                
            }
            return false;
        }

        public async Task DeleteCategoryAsync(TestCategory testCategory)
        {
            var testCategoryObj = await _dbContext.TestCategory.FirstOrDefaultAsync(x => x.TestId == testCategory.TestId && x.CategoryId == testCategory.CategoryId);
            _dbContext.TestCategory.Remove(testCategoryObj);
            await _dbContext.SaveChangesAsync();
            var category = await _dbContext.Category.FirstAsync(x => x.Id == testCategoryObj.CategoryId);
            var categoryAc = Mapper.Map<Category, CategoryAC>(category);
            categoryAc.IsSelect = false;
        }
        #endregion

    }
}