using Promact.Trappist.DomainModel.DbContext;
using System.Linq;
using Promact.Trappist.DomainModel.Models.Test;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.Utility.Constants;


namespace Promact.Trappist.Repository.Tests
{
    public class TestsRepository : ITestsRepository
    {
        private static Random random = new Random();
        private readonly TrappistDbContext _dbContext;
        private readonly IStringConstants _stringConstants;
        public TestsRepository(TrappistDbContext dbContext, IStringConstants stringConstants)
        {
            _dbContext = dbContext;
            _stringConstants = stringConstants;
        }
        /// <summary>
        /// this method is used to create a new test
        /// </summary>
        /// <param name="test">object of Test</param>
        public async Task CreateTestAsync(Test test)
        {
            _dbContext.Test.Add(test);
             await _dbContext.SaveChangesAsync();            
        }
        /// <summary>
        /// Fetch all the tests from Test Model,Convert it into List
        /// </summary>
        /// <returns>List of Tests</returns>   
        public List<Test> GetAllTests()
        {
            var tests = _dbContext.Test.ToList();
            return tests;
        }

        /// <summary>
        ///  this method is used to generate a random string for every test at the time of test creation
        /// <param name="test">object of Test</param>
        /// <param name="length">length of the random string</param>
        /// </summary>
        public void RandomLinkString(Test test, int length)
        {
            string charactersForRandomString = _stringConstants.CharactersForLink;
            test.Link = new string(Enumerable.Repeat(charactersForRandomString, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// this method is used to check whether test name is unique or not
        /// </summary>
        /// <param name="test">object of Test</param>
        /// <returns>boolean</returns>
        public async Task<bool> IsTestNameUniqueAsync(string testName)
        {
            bool testNameCheck = await _dbContext.Test.AnyAsync(x => x.TestName.ToLower() == testName.ToLower());
            return testNameCheck;            
        }
        /// <summary>
        /// Fetch all the tests from Test Model,Convert it into List
        /// </summary>
        /// <returns>List of Testsby decreasing order of there created Date</returns>
        public async Task<List<Test>> GetAllTestsAsync()
        {
            return await _dbContext.Test.OrderByDescending(x => x.CreatedDateTime).ToListAsync();
        }
    }
}