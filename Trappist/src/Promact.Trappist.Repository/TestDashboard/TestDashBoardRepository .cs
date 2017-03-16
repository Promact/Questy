using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Test;
using System.Collections.Generic;
using System.Linq;

namespace Promact.Trappist.Repository.TestDashBoard
{
    public class TestDashBoardRepository : ITestDashBoardRepository
    {

        private readonly TrappistDbContext _trappistdb;
        public TestDashBoardRepository(TrappistDbContext trappistdb)
        {
            _trappistdb = trappistdb;
        }
        /// <summary>
        /// Fetch all the tests from Test Model,Convert it into List
        /// </summary>
        /// <returns>List of Tests</returns>
        public List<Test> GetAllTests()
        {
            var tests = _trappistdb.Test.ToList();
            return tests;
        }
    }
}
