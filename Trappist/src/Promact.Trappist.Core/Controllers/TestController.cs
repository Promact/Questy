﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/tests")]
    public class TestsController : Controller
    {
        #region Private Members
        private readonly ITestsRepository _testRepository;
        private readonly IStringConstants _stringConstants;
        #endregion

        #region Constructor
        public TestsController(ITestsRepository testRepository, IStringConstants stringConstants)
        {
            _testRepository = testRepository;
            _stringConstants = stringConstants;
        }
        #endregion

        #region Test
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
        #endregion 
        #region Test Settings
        /// <summary>
        /// Gets the Settings saved for a particular Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to get the Settings of a Test by its Id</param>
        /// <returns>Settings saved for the selected Test</returns>
        [HttpGet("{id}/settings")]
        public async Task<IActionResult> GetTestByIdAsync([FromRoute] int id)
        {
            var testSettings = await _testRepository.GetTestByIdAsync(id);
            if (testSettings == null)
                return NotFound();
            return Ok(testSettings);
        }

        /// <summary>
        /// Updates the edited Test Name
        /// </summary>
        /// <param name="id">The parameter "id" is used to access the name of the selected Test</param>
        /// <param name="testObject">The parameter "testObject" is an Object of Test</param>
        /// <returns>Updated Test Name</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTestNameAsync([FromRoute]int id, [FromBody] Test testObject)
        {
            if (!ModelState.IsValid || testObject == null)
                return BadRequest();
            if (!await _testRepository.IsTestExists(id))
                return NotFound();
            await _testRepository.UpdateTestNameAsync(id, testObject);
            return Ok(testObject);
        }

        /// <summary>
        /// Updates the changes made to the settings of a Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to access the Settings of that Test</param>
        /// <param name="testObject">The parameter "testObject" is an object of Test</param>
        /// <returns>Updated Settings of that Test</returns>     
        [HttpPut("{id}/settings")]
        public async Task<IActionResult> UpdateTestByIdAsync([FromRoute] int id, [FromBody] Test testObject)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(_stringConstants.ErrorKey, _stringConstants.TestNameInvalidError);
                return BadRequest(ModelState);
            }
            if (testObject == null)
                return BadRequest();
            if (!await _testRepository.IsTestExists(id))
                return NotFound();
            if (!await _testRepository.IsTestNameUniqueAsync(testObject.TestName, id))
            {
                ModelState.AddModelError(_stringConstants.ErrorKey, _stringConstants.TestNameExistsError);
                return BadRequest(ModelState);
            }
            await _testRepository.UpdateTestByIdAsync(testObject);
            return Ok(testObject);
        }
        #endregion
        #region Delete Test
        /// <summary>
        /// Check whether there is any test attendee or not
        /// </summary>
        /// <param name="id">Id of the test</param>
        /// <returns>Ok if an attendee exist else BadRequest</returns>
        [HttpGet("{id}/testAttendee")]
        public async Task<IActionResult> IsTestAttendeeExistAsync(int id)
        {
            if (await _testRepository.IsTestAttendeeExistAsync(id))
                return Ok(id);
            return BadRequest();
        }

        /// <summary>
        /// Delete the test from the database
        /// </summary>
        /// <param name="id">Id of the test to be deleted</param>
        /// <returns>NoContent if test is deleted else BadRequest or NotFound</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!await _testRepository.IsTestExists(id))
            {
                return NotFound();
            }
            if (await _testRepository.IsTestAttendeeExistAsync(id))
            {
                return NotFound();
            }
            await _testRepository.DeleteTestAsync(id);
            return NoContent();
        }

        [HttpGet("questions/{testid}/{categoryid}")]
        public async Task<IActionResult> GetTestCategoryQuestionsByIdAsync([FromRoute] int  testid,[FromRoute] int categoryid)
        {
            return Ok( await _testRepository.GetAllTestCategoryQuestionsByIdAsync(testid, categoryid));
        }
        [HttpPost("questions/{testId}")]
        public async Task<IActionResult> AddTestQuestionsAsync([FromBody] List<QuestionAC> QuestionToAddTest,[FromRoute] int testId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
            {
                var message = await _testRepository.AddTestQuestionsAsync(QuestionToAddTest, testId);
                return Ok(new {message = message});
            }
        }
        [HttpGet("testDetails/{id}")]
        public async Task<IActionResult> GetTestDetailsByIdAsync([FromRoute] int id)
        {          
            return Ok(await _testRepository.GetTestDetailsByIdAsync(id));
        }
        #endregion
    }
}