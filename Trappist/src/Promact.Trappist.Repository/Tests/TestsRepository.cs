using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Test;
using System.Linq;
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
        /// service for new test
        /// </summary>
        /// <param name="test">a variable of type 'Test'</param>
        public void CreateTest(Test test)
        {
          _dbContext.Test.Add(test);
           _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// to check whether test name is unique or not
        /// </summary>
        /// <param name="testObj">a variable of type 'Test'</param>
        /// <returns>it will return boolean value, true if name is unique else false</returns>
        public bool UniqueName(Test testObj)
        {
            var unique = (from s in _dbContext.Test
                          where s.TestName == testObj.TestName
                          select s).FirstOrDefault();
            if (unique != null)
                return false;
            else
                return true;

        }
    }
}
