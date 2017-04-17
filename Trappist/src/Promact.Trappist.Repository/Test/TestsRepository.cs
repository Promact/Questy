using Promact.Trappist.DomainModel.DbContext;
using System.Linq;
using Promact.Trappist.DomainModel.Models.Test;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.Utility.GlobalUtil;
using System;
using AutoMapper;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.Utility.ExtensionMethods;

namespace Promact.Trappist.Repository.Tests
{
    public class TestsRepository : ITestsRepository
    {
        public List<CategoryAC> categorylist = new List<CategoryAC>();
        public List<Category> categories = new List<Category>();
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

        /// <summary>
        /// this method is used to check whether test name is unique or not
        /// </summary>
        /// <param name="test">object of Test</param>
        /// <returns>boolean</returns>
        public async Task<bool> IsTestNameUniqueAsync(string testName, int id)
        {
            testName = System.Text.RegularExpressions.Regex.Replace(testName, @"\s+", " "); ;
            //string = string.replace(/\s\s +/ g, ' ');
            var isTestExists = await (_dbContext.Test.AnyAsync(x =>
                                    x.TestName.ToLowerInvariant() == testName.AllTrim().ToLowerInvariant()
                                    && x.Id != id));
            return !isTestExists;
        }

        /// <summary>
        /// Fetch all the tests from Test Model,Convert it into List
        /// </summary>
        /// <returns>List of Testsby decreasing order of there created Date</returns>
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
            testSettingsToUpdate.TestName = testObject.TestName;
            _dbContext.Test.Update(testSettingsToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the changes made to the settings of a Test
        /// </summary>
        /// <param name="testObject">The parameter "testObject" is an object of Test</param>
        /// <returns>Updated Settings of that Test</returns>
        public async Task UpdateTestSettingsAsync(Test testObject)
        {
            _dbContext.Test.Update(testObject);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the Settings saved for a particular Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to get the Settings of a Test by its Id</param>
        /// <returns>Settings Saved for the selected Test</returns>
        public async Task<Test> GetTestSettingsAsync(int id)
        {
            string currentDate = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm");
            var testSettings = await _dbContext.Test.FirstOrDefaultAsync(x => x.Id == id);
            if (testSettings != null)
            {
                //testSettings.StartDate = testSettings.StartDate == default(DateTime) ? Convert.ToDateTime(currentDate) : testSettings.StartDate; //If the StartDate field in database contains default value on visiting the Test Settings page of a Test for the first time then that default value gets replaced by current DateTime
                // testSettings.EndDate = testSettings.EndDate == default(DateTime) ? Convert.ToDateTime(currentDate) : testSettings.EndDate; //If the EndDate field in database contains default value on visiting the Test Settings page of a Test for the first time then that default value gets replaced by current DateTime
                return testSettings;
            }
            else
                return null;
        }

        /// <summary>
        /// Checks if the Test Settings Exists or not
        /// </summary>
        /// <param name="id">The parameter "id" is taken from the route</param>
        /// <returns>A bool value based on the condition is satisfied or not</returns>
        public async Task<bool> IsTestExists(int id)
        {
            return await _dbContext.Test.AnyAsync(x => x.Id == id);
        }
        #endregion
        #region Category selection
        public async Task<TestAC> GetTestDetailsByIdAsync(int id)
        {
            var testobj = await _dbContext.Test.FindAsync(id);
            var testACObj = Mapper.Map<Test, TestAC>(testobj);
            testACObj.Category = new List<CategoryAC>();
            var tests = _dbContext.Test.ToList();
            var testCategories = _dbContext.TestCategory.Where(x => x.TestId == id).ToList();
            List<Category> categoryList = _dbContext.Category.ToList();
            categorylist = Mapper.Map<List<Category>, List<CategoryAC>>(categoryList);
            foreach (var category in categorylist)
            {
                if (testCategories.Exists(x => x.CategoryId == category.Id))
                    category.IsSelect = true;

            }

            testACObj.Category = categorylist;
            return testACObj;
        }
        public async Task AddSelectedCategoryAsync(List<TestCategory> testCategory)
        {
            List<TestCategory> testCategoryList = new List<TestCategory>();
            var testCategories = await _dbContext.TestCategory.ToListAsync();
            foreach (var category in testCategory)
            {
                if (!testCategories.Exists(x => x.CategoryId == category.CategoryId))
                    testCategoryList.Add(category);
            }
            await _dbContext.TestCategory.AddRangeAsync(testCategoryList);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeselectCategoryAsync(int id)
        {
            var testCategory = await _dbContext.TestCategory.FindAsync(id);
            _dbContext.TestCategory.Remove(testCategory);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

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
    }
}