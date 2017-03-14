using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.Repository.Tests;
namespace Promact.Trappist.Core.Controllers
{
    [Produces("application/json")]
    [Route("api/tests")]
    public class TestsController : Controller
    {
        private ITestsRepository _test;
        public TestsController(ITestsRepository test)
        {
            _test = test;
        }   
        /// <summary>
        /// adding a new test after checking unique name
        /// </summary>
        /// <param name="test">object of the Test model is passed</param>
        /// <returns></returns>
        [HttpPost]
        public string TestCreate([FromBody] Test test)
        {
            if (_test.UniqueName(test) == false)
                return "Invalid Test Name";

            _test.CreateTest(test);
            return "succes";

        }
    }
}