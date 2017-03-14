using Promact.Trappist.DomainModel.DbContext;
using System.Linq;
using Promact.Trappist.DomainModel.Models.Test;

namespace Promact.Trappist.Repository.Tests
{
    public class TestsRepository : ITestsRepository
    {
        private readonly TrappistDbContext _dbContext;
        public TestsRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// this method is used to create a new test
        /// </summary>
        /// <param name="test">object of Test</param>
        public void CreateTest(Test test)
        {
            _dbContext.Test.Add(test);
            _dbContext.SaveChangesAsync();
        }      
        /// <summary>
        /// this method is used to check whether test name is unique or not
        /// </summary>
        /// <param name="test">object of Test</param>
        /// <returns>boolean</returns>
        public bool UniqueTestName(Test test)
        {
            var testObj = (from s in _dbContext.Test
                          where s.TestName == test.TestName
                          select s).FirstOrDefault();
            if (testObj != null)
                return false;
            else
                return true;
        }
    }
}