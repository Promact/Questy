using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.Repository.Tests;

namespace Promact.Trappist.Core.Controllers
{
    [Produces("application/json")]
    [Route("api/tests")]
    public class TestsController : Controller
    {
        private ITestsRepository _test;
        private readonly TrappistDbContext _context;
        public TestsController(ITestsRepository test)
        {
            _test = test;           
        }
        /// <summary>
        /// adding a new test after checking name is unique or not
        /// </summary>
        /// <param name="test">new test will be added to the model</param>
        /// <returns></returns>
        [HttpPost]
        public string AddTest([FromBody] Test test)
        {
            if (_test.UniqueName(test) == false)
                return "Invalid Test Name";

            _test.CreateTest(test);
            return "succes";

        }

    }


}