using Promact.Trappist.DomainModel.DbContext;
using System.Linq;
using Promact.Trappist.DomainModel.Models.Test;
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
        
        public async Task CreateTestAsync(Test test)
        {           
            _dbContext.Test.Add(test);
            test.Link = _util.RandomLinkGenerator(10);
            await _dbContext.SaveChangesAsync();
        }                       
        
        public async Task<bool> IsTestNameUniqueAsync(string testName)
        {
            bool testNameCheck;
            testNameCheck = await _dbContext.Test.AnyAsync(x => x.TestName.ToLowerInvariant() == testName.ToLowerInvariant());
            return testNameCheck;
        }
        
        public async Task<List<Test>> GetAllTestsAsync()
        {
            return await _dbContext.Test.OrderByDescending(x => x.CreatedDateTime).ToListAsync();
        }
    }
}