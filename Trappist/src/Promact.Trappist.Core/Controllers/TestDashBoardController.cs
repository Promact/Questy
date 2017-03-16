using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.TestDashBoard;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/TestDashboard")]
    public class TestDashBoardController : Controller
    {
            public ITestDashBoardRepository _testDashBoardRepository;
            public TestDashBoardController(ITestDashBoardRepository testDashBoardRepository)
            {
                _testDashBoardRepository = testDashBoardRepository;
             
            }
            /// <summary>
            /// Get All Tests
            /// </summary>
            /// <returns>List of Tests</returns>
            [HttpGet]
            public IActionResult GetAllTest()
            {
                var Tests = _testDashBoardRepository.GetAllTests();
                return Json(Tests);
            }
        }
}
