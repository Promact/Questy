﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.Utility.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.Repository.Test;

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
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructor
        public TestsController(ITestsRepository testRepository, IStringConstants stringConstants, UserManager<ApplicationUser> userManager)
        {
            _testRepository = testRepository;
            _stringConstants = stringConstants;
            _userManager = userManager;
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
            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (ModelState.IsValid)
            {
                await _testRepository.CreateTestAsync(test, applicationUser.Id);
                return Ok(test);
            }

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
        /// Updates the edited Test Name
        /// </summary>
        /// <param name="id">The parameter "id" is used to access the name of the selected Test</param>
        /// <param name="testObject">The parameter "testObject" is an Object of Test</param>
        /// <returns>Updated Test Name</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTestNameAsync([FromRoute]int id, [FromBody] Test testObject)
        {
            if (!ModelState.IsValid || testObject == null || !await _testRepository.IsTestEditableAsync(id))
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
        [HttpGet("isPausedResume/{id}/{isPause}")]
        public async Task<IActionResult> PauseTestAsync([FromRoute] int id, [FromRoute] bool isPause)
        {
            await _testRepository.PauseResumeTestAsync(id, isPause);
            return Ok(isPause);
        }

        [HttpDelete("deleteIp/{id}")]
        public async Task<IActionResult> DeleteIpAsync([FromRoute] int id)
        {
            await _testRepository.DeleteIpAddressAsync(id);
            return Ok();
        }
        #endregion

        #region Delete Test
        /// <summary>
        /// Check whether there is any test attendee or not
        /// </summary>
        /// <param name="id">Id of the test</param>
        /// <returns>Ok if an attendee exist with response of true and false</returns>
        [HttpGet("{id}/testAttendee")]
        public async Task<IActionResult> IsTestAttendeeExistAsync(int id)
        {
            var isAttendeeExist = await _testRepository.IsTestAttendeeExistAsync(id);
            return Ok(isAttendeeExist);
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

        #region Category Selection

        /// <summary>
        /// this method is used to add the selected categories from category list to the TestCategory model
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="categoryAcList"></param>
        /// <returns>testCategory</returns>
        [HttpPost("addTestCategories/{testId}")]
        public async Task<ActionResult> AddTestCategoriesAsync([FromRoute] int testId, [FromBody] List<TestCategoryAC> categoryAcList)
        {
            await _testRepository.AddTestCategoriesAsync(testId, categoryAcList);
            return Ok(true);
        }

        /// <summary>
        /// this method is used to check whether question from selected category is added in test or not
        /// </summary>
        /// <param name="categoryId">Id of the category to be deselected</param>
        /// <param name="testId"></param>
        /// <returns>boolean</returns>
        [HttpGet("deselectCategory/{categoryId}/{testId}")]
        public async Task<bool> DeselectCategoryAsync([FromRoute] int categoryId, [FromRoute] int testId)
        {
            var isQuestionExists = await _testRepository.DeselectCategoryAync(categoryId, testId);
            return isQuestionExists;
        }

        /// <summary>
        /// this method is used to deselect a category from test and also removes
        /// </summary>
        /// <param name="testCategory"></param>
        /// <returns>testCategory</returns>
        [HttpPost("deselectCategory")]
        public async Task<ActionResult> RemoveCategoryAndQuestionAsync([FromBody] TestCategory testCategory)
        {
            await _testRepository.RemoveCategoryAndQuestionAsync(testCategory);
            return Ok(testCategory);
        }
        #endregion

        #region Test-Question-Selection
        /// <summary>
        /// Gets all the questions present in a category by its id
        /// </summary>
        /// <param name="testId">Id of test in which category is present</param>
        /// <param name="categoryId">Id of category whose questions would be fetched</param>
        /// <returns>List of questions</returns>
        [HttpGet("questions/{testid}/{categoryid}")]
        public async Task<IActionResult> GetTestCategoryQuestionsByIdAsync([FromRoute] int testId, [FromRoute] int categoryId)
        {
            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            return Ok(await _testRepository.GetAllQuestionsByTestIdAsync(testId, categoryId, applicationUser.Id));
        }

        /// <summary>
        /// Adds the selected question to TestQuestion Model
        /// </summary>
        /// <param name="questionToAdd"></param>
        /// <param name="testId">id of test in which questions will be added</param>
        /// <returns>String message if successfull</returns>
        [HttpPost("questions/{testId}")]
        public async Task<IActionResult> AddTestQuestionsAsync([FromBody] List<TestQuestionAC> questionToAdd, [FromRoute] int testId)
        {
            if (!ModelState.IsValid && await _testRepository.IsTestAttendeeExistAsync(testId))
                return BadRequest(ModelState);
            var message = await _testRepository.AddTestQuestionsAsync(questionToAdd, testId);
            return Ok(new { message = message });
        }

        /// <summary>
        /// Gets all the details of a test
        /// </summary>
        /// <param name="id">id of test</param>
        /// <returns>Object of TestAC</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestByIdAsync([FromRoute] int id)
        {
            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            var testAcObject = await _testRepository.GetTestByIdAsync(id, applicationUser.Id);
            if (testAcObject == null)
                return NotFound();
            return Ok(testAcObject);
        }
        #endregion

        #region Duplicate Test

        /// <summary>
        /// Duplicates questions,categories and Ip addresses present in the test to be duplicated
        /// </summary>
        /// <returns>Test object for the duplicated test</returns>
        [HttpPost("{id}/duplicateTest")]
        public async Task<IActionResult> DuplicateTest([FromRoute]int id, [FromBody]Test test)
        {
            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            return Ok(await _testRepository.DuplicateTest(id, test));
        }

        /// <summary>
        /// Sets the number of times the test has been duplicated
        /// </summary>
        /// <param name="testName">Contains the name of the test which is to be duplicated</param>
        /// <returns>The number of times the test has been duplicated</returns>
        [HttpGet("{testName}/setTestCopiedNumber")]
        public async Task<IActionResult> SetTestCopiedNumberAsync([FromRoute]string testName)
        {
            return Ok(await _testRepository.SetTestCopiedNumberAsync(testName));
        }
        #endregion
    }
}