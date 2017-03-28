using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Produces("application/json")]
    [Route("api/tests")]
    public class TestsController : Controller
    {
        private readonly ITestsRepository _testRepository;
        private readonly IStringConstants _stringConstant;

        public TestsController(ITestsRepository testRepository, IStringConstants stringConstant)
        {
            _testRepository = testRepository;
            _stringConstant = stringConstant;
        }
        /// <summary>
        /// this method is used to add a new test 
        /// </summary>
        /// <param name="test">object of the Test</param>
        /// <returns>string</returns>
        [HttpPost]
        public string CreateTest([FromBody] Test test)
        {
            if (_testRepository.UniqueTestName(test)) // verifying the test name is unique or not
            {
                _testRepository.CreateTest(test);
                return _stringConstant.Success;
            }
            else
            {
                return _stringConstant.InvalidTestName;
            }
        }
        /// <summary>
        /// Get All Tests
        /// </summary>
        /// <returns>List of Tests</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllTest()
        {
            var tests = await _testRepository.GetAllTestsAsync();
            return Ok(tests);
        }
    }
}