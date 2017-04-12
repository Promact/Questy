using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.TestConduct;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/conduct")]
    public class TestConductController : Controller
    {
        #region Private Variables
        #region Dependencies
        private readonly ITestConductRepository _testConductRepository;
        #endregion
        #endregion

        #region Constructor
        public TestConductController(ITestConductRepository testConductRepository)
        {
            _testConductRepository = testConductRepository;
        }
        #endregion

        #region Public method
        /// <summary>
        /// This method used for register test attendee for the current test.
        /// </summary>
        /// <param name="model">This model object contain test attendee credential which are first name, last name, email, roll number, contact number</param>
        /// <param name="magicString">This parameter contain test link</param>
        /// <returns>It will return true if test attendee successfully registered else it will return false.</returns>
        [HttpPost("{magicString}/register")]
        public async Task<IActionResult> RegisterTestAttendeesAsync([FromBody]TestAttendees model, [FromRoute]string magicString)
        {
            if (ModelState.IsValid)
                return Ok(await _testConductRepository.RegisterTestAttendeesAsync(model, magicString));
            return BadRequest();
        }
        #endregion
    }
}