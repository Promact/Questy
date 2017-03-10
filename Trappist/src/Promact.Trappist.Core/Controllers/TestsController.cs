using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Web.Data;

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
            //_context = context;
        }
        [HttpGet]
        public string test1()
        {
            return "test created";
        }
        /// <summary>
        /// adding a new test after checking unique name
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        [HttpPost]
        public string testPost([FromBody] Test test)
        {
            if (_test.uniqueName(test) == false)
                return "Invalid Test Name";

            _test.createTest(test);
            return "succes";

        }

    }


}