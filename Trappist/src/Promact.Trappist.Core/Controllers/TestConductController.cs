using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Utility.Constants;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/conduct")]
    public class TestConductController : Controller
    {
        #region Private Variables
        #region Dependencies
        private readonly ITestConductRepository _testConductRepository;
        private readonly IStringConstants _stringConstants;
        #endregion
        #endregion

        #region Constructor
        public TestConductController(ITestConductRepository testConductRepository, IStringConstants stringConstants)
        {
            _testConductRepository = testConductRepository;
            _stringConstants = stringConstants;
        }
        #endregion

        #region Public method
        /// <summary>
        /// This method used for register test attendee for the current test.
        /// </summary>
        /// <param name="testAttendee">This model object contain test attendee credential which are first name, last name, email, roll number, contact number</param>
        /// <param name="magicString">This parameter contain test link</param>
        /// <returns>It will return true response if test attendee successfully registered else it will return bad request.</returns>
        [HttpPost("{magicString}/register")]
        public async Task<IActionResult> RegisterTestAttendeesAsync([FromBody]TestAttendees testAttendee, [FromRoute]string magicString)
        {
            if (ModelState.IsValid && !(await _testConductRepository.IsTestAttendeeExistAsync(testAttendee, magicString)))
            {
                await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, magicString);
                HttpContext.Session.SetString(_stringConstants.SessionKeyEmail, testAttendee.Email);
                HttpContext.Session.SetString(_stringConstants.SessionKeyRollNumber, testAttendee.RollNumber);
                HttpContext.Session.SetInt32(_stringConstants.SessionKeyId, testAttendee.Id);
                return Ok(true);
            }
            return BadRequest();
        }
        #endregion
    }
}