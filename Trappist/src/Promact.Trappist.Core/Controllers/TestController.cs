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
       private IStringConstants _stringConstant;
            
        public TestsController(ITestsRepository test,IStringConstants stringConstant)
        {
            _test = test;
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
            if (_test.UniqueTestName(test)) // verifying the test name is unique or not
            {
                _test.CreateTest(test);
                return _stringConstant.Success;              
            }
            else
            {
                return _stringConstant.InvalidTestName;
            }           
        }
    }
}