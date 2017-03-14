using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;

namespace Promact.Trappist.Core.Controllers
{
    [Produces("application/json")]
    [Route("api/tests")]
    public class TestsController : Controller
    {
        private ITestsRepository _test;
        private IStringConstants _message;
            
        public TestsController(ITestsRepository test,IStringConstants message)
        {
            _test = test;
            _message = message;
        }    
        /// <summary>
        /// Method to add a new test 
        /// </summary>
        /// <param name="test">object of the Test</param>
        /// <returns>string</returns>
        [HttpPost]
        public string TestCreate([FromBody] Test test)
        {
            if (_test.UniqueName(test) == false) //method verifying the test name is unique or not
             return _message.InvalidTestName;

            _test.CreateTest(test);
                return _message.Success;

        }
    }
}