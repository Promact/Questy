using Promact.Trappist.DomainModel.DbContext;
using System.Linq;
using Promact.Trappist.DomainModel.Models.Test;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.Utility.GlobalUtil;

namespace Promact.Trappist.Repository.Tests
{
    public class TestsRepository : ITestsRepository
    {
        private readonly TrappistDbContext _dbContext;
        private readonly IUtil _util;
        public TestsRepository(TrappistDbContext dbContext, IUtil util)
        {
            _dbContext = dbContext;
            _util = util;
        }
        /// <summary>
        /// this method is used to create a new test
        /// </summary>
        /// <param name="test">object of Test</param>
        public async Task CreateTestAsync(Test test)
        {           
            _dbContext.Test.Add(test);
            test.Link = _util.RandomLinkGenerator(10);
            await _dbContext.SaveChangesAsync();
        }                
        
        /// <summary>
        /// this method is used to check whether test name is unique or not
        /// </summary>
        /// <param name="test">object of Test</param>
        /// <returns>boolean</returns>
        public async Task<bool> IsTestNameUniqueAsync(string testName)
        {
            bool testNameCheck;
            testNameCheck = await _dbContext.Test.AnyAsync(x => x.TestName.ToLowerInvariant() == testName.ToLowerInvariant());
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