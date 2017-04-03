using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.TestSettings;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.Repository.Tests;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Produces("application/json")]
    [Route("api/tests")]
    public class TestsController : Controller
    {
        private readonly ITestsRepository _testRepository;
        public TestsController(ITestsRepository testRepository)
        {
            _testRepository = testRepository;
        }
        /// <summary>
        /// this method is to verify unique name of a test
        /// </summary>
        /// <param name="testName">name of the test</param>
        /// <returns>boolean</returns>

        [HttpGet("isUnique/{testName}/{id}")]
        public async Task<bool> IsTestNameUnique([FromRoute] string testName, [FromRoute] int id)
        {
            // verifying the test name is unique or not
            return await _testRepository.IsTestNameUniqueAsync(testName, id);
        }
        /// <summary>
        /// this method is used to add a new test 
        /// </summary>
        /// <param name="test">object of the Test</param>   
        /// <returns>created test</returns>
        [HttpPost]
        public async Task<IActionResult> CreateTest([FromBody] Test test)
        {
            if (ModelState.IsValid)
            {
                await _testRepository.CreateTestAsync(test);
                return Ok(test);
            }
            else
                return BadRequest();
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

        /// <summary>
        /// Gets the Settings saved for a particular Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to get the Settings of a Test by its Id</param>
        /// <returns>Settings saved for the selected Test</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestSettings([FromRoute] int id)
        {
            var settings = await _testRepository.GetTestSettingsAsync(id);
            if (settings == null)
                return NotFound();
            else
                return Ok(settings);
        }

        /// <summary>
        /// Updates the changes made to the settings of a Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to access the Settings of that Test</param>
        /// <param name="testSettingsAC">The parameter "testSettingsAC" is an object of TestSettingsAC</param>
        /// <returns>Updated Settings of that Test</returns>     
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTestSettings([FromRoute] int id, [FromBody] TestSettingsAC testSettingsAC)
        {
            if (await _testRepository.TestSettingsExists(id))
                return NotFound();
            if (ModelState.IsValid)
            {
                await _testRepository.UpdateTestSettingsAsync(testSettingsAC);
                return Ok(testSettingsAC);
            }
            else
                return BadRequest();
        }
    }
}