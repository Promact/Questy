using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.TestDashBoard;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/TestDashboard")]
    public class TestDashBoardController : Controller
    {
      
            public ITestDashBoardRepository _testdashboardRepository;  
            public TestDashBoardController(ITestDashBoardRepository testdashboardRepository)
            {
                 _testdashboardRepository = testdashboardRepository;          
            }
            /// <summary>
            /// Get All Tests
            /// </summary>
            /// <returns>List of Tests</returns>
            [HttpGet]
            public IActionResult GetAllTest()
            {
                var Tests = _testdashboardRepository.GetAllTests();
                return Json(Tests);
            }
        }
}
