﻿using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{testName}")]
        public async Task<IActionResult> IsUniqueTestName([FromRoute] string testName)
        {
            // verifying the test name is unique or not
            bool isExist = await _testRepository.IsTestNameUniqueAsync(testName);
            return Ok(isExist);          
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
                _testRepository.RandomLinkString(test, 10);
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
    }
}