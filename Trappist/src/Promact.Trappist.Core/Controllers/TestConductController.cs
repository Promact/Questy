using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Repository.Tests;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Authorize]
    [Route("api/conduct")]
    public class TestConductController : Controller
    {
        #region Private Variables
        #region Dependencies
        private readonly ITestConductRepository _testConductRepository;
        private readonly ITestsRepository _testRepository;
        #endregion
        #endregion

        #region Constructor
        public TestConductController(ITestConductRepository testConductRepository, ITestsRepository testRepository)
        {
            _testConductRepository = testConductRepository;
            _testRepository = testRepository;
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
                return Ok(true);
            }
            return BadRequest();
        }

        /// <summary>
        /// Adds answer to the Database as a Key-Value pair
        /// </summary>
        /// <param name="attendeeId">Id of Test Attendee</param>
        /// <param name="answer">Answer submitted</param>
        /// <returns></returns>
        [HttpPut("answer/{attendeeId}")]
        public async Task<IActionResult> AddAnswerAsync([FromRoute]int attendeeId, [FromBody]string answer)
        {
            if(answer.Length == 0)
            {
                return BadRequest();
            }
            if(!await _testRepository.IsTestAttendeeExistAsync(attendeeId))
            {
                return NotFound();
            }
            await _testConductRepository.AddAnswerAsync(attendeeId, answer);

            return Ok(answer);
        }

        [HttpGet("answer/{attendeeId}")]
        public async Task<IActionResult> GetAnswerAsync([FromRoute]int attendeeId)
        {
            if(!await _testRepository.IsTestAttendeeExistAsync(attendeeId))
            {
                return NotFound();
            }

            return Ok(await _testConductRepository.GetAnswerAsync(attendeeId));
        }

        #region Test-Instruction API
        /// <summary>
        /// This method is used to get all the instructions before starting of a particular test using testLink
        /// </summary>
        /// <param name="testLink">link to conduct a particular test</param>
        /// <returns>instructions for a particular test</returns>
        [HttpGet("{testLink}/instructions")]
        public async Task<TestInstructionsAC> GetTestInstructionsAsync(string testLink)
        {
            var result = await _testConductRepository.GetTestInstructionsAsync(testLink);
            return result;
        }
        #endregion
        #endregion
    }
}