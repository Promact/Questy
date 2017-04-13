﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.Repository.Tests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/tests")]
    public class TestsController : Controller
    {
        private readonly ITestsRepository _testRepository;
        public TestsController(ITestsRepository testRepository)
        {
            _testRepository = testRepository;
        }

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
        public async Task<IActionResult> GetTestSettings([FromRoute] int id)
        {
            var testSettings = await _testRepository.GetTestSettingsAsync(id);
            if (testSettings == null)
                return NotFound();
            else
                return Ok(testSettings);
        }

        /// <summary>
        /// Updates the edited Test Name
        /// </summary>
        /// <param name="id">The parameter "id" is used to access the name of the selected Test</param>
        /// <param name="testObject">The parameter "testObject" is an Object of Test</param>
        /// <returns>Updated Test Name</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTestName([FromRoute]int id, [FromBody] Test testObject)
        {
            if (!await _testRepository.IsTestExists(id))
                return NotFound();
            if (ModelState.IsValid)
            {
                await _testRepository.UpdateTestNameAsync(id, testObject);
                return Ok(testObject);
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Updates the changes made to the settings of a Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to access the Settings of that Test</param>
        /// <param name="testObject">The parameter "testObject" is an object of Test</param>
        /// <returns>Updated Settings of that Test</returns>     
        [HttpPut("{id}/settings")]
        public async Task<IActionResult> UpdateTestSettings([FromRoute] int id, [FromBody] Test testObject)
        {
            if (!await _testRepository.IsTestExists(id))
                return NotFound();
            if (ModelState.IsValid)
            {
                await _testRepository.UpdateTestSettingsAsync(testObject);
                return Ok(testObject);
            }
            else
                return BadRequest();
        }
#endregion
#region Category Selection
        [HttpPost("selectedCategories")]
        public async Task<ActionResult> GetTestDetail(List<CategoryAC> catelist)
        {
          await _testRepository.GetTestDetailsAsync(catelist);
            return Ok(catelist);
        }

        [HttpDelete("deselectCategory/{id}")]
        public async Task<ActionResult> DeselectCategoriesAsync([FromRoute] int id)
        {
            await _testRepository.DeselectCategoryAsync(id);
            return Ok();
        }
        #endregion

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
        #endregion
    }
}