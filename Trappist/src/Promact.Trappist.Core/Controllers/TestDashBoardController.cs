using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.TestDashBoard;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/TestDashboard")]
    public class TestDashBoardController : Controller
    {
      
     
            public ITestDashBoardRepository _testDisplay;
           
            public TestDashBoardController(ITestDashBoardRepository testDisplay)
            {
                _testDisplay = testDisplay;
             
            }

            /// <summary>
            /// Get All Tests
            /// </summary>
            /// <returns>List of Tests</returns>
            [HttpGet]
            public IActionResult GetAllTest()
            {
                var Tests = _testDisplay.GetAllTests();

                return Json(Tests);
            }


        }
}
