using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Test;
using System.Linq;
namespace Promact.Trappist.Repository.Tests
{
    public class TestsRepository : ITestsRepository
    {
        // List<Test> t_list = new List<Test>();
        private readonly TrappistDbContext _dbContext;
        public TestsRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        /// <summary>
        /// service for new test
        /// </summary>
        /// <param name="t1"></param>


        public void createTest(Test t1)
        {

            _dbContext.Test.Add(t1);

            _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// to check whether test name is unique or not
        /// </summary>
        /// <param name="t"></param>
        /// <returns>boolean</returns>
        public bool uniqueName(Test t)
        {
            var unique = (from s in _dbContext.Test
                          where s.TestName == t.TestName
                          select s).FirstOrDefault();
            if (unique != null)
                return false;
            else
                return true;

        }
    }
}
