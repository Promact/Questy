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
        /// to create a new test
        /// </summary>
        /// <param name="test">object of Test is passed</param>
        public void CreateTest(Test test)
        {
            _dbContext.Test.Add(test);

            _dbContext.SaveChangesAsync();
        }      
        /// <summary>
        /// to check whether test name is unique or not
        /// </summary>
        /// <param name="test"></param>
        /// <returns>boolean</returns>
        public bool UniqueName(Test test)
        {
            var unique = (from s in _dbContext.Test
                          where s.TestName == test.TestName
                          select s).FirstOrDefault();
            if (unique != null)
                return false;
            else
                return true;

        }
    }
}