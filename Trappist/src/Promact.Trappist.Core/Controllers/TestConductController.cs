using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Repository.Tests;
using System;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/conduct")]
    public class TestConductController : Controller
    {
        #region Private Variables
        #region Dependencies
        private readonly ITestConductRepository _testConductRepository;
        private readonly ITestsRepository _testRepository;
        private const string ATTENDEE_ID = "ATTENDEE_ID";
        private const string TEST_LINK = "MAGIC_LINK";
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
                HttpContext.Session.SetInt32(ATTENDEE_ID, testAttendee.Id);
                HttpContext.Session.SetString(TEST_LINK, magicString);
                return Ok(true);
            }
            return BadRequest();
        }

        /// <summary>
        /// Adds answer to the Database as a Key-Value pair
        /// </summary>
        /// <param name="attendeeId">Id of Test Attendee</param>
        /// <param name="answer">Answer submitted</param>
        [HttpPut("answer/{attendeeId}")]
        public async Task<IActionResult> AddAnswerAsync([FromRoute]int attendeeId, [FromBody] TestAnswerAC[] answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (HttpContext.Session.GetInt32(ATTENDEE_ID).Value != attendeeId)
            {
                return BadRequest();
            }

            if (!await _testConductRepository.IsTestAttendeeExistByIdAsync(attendeeId))
            {
                return NotFound();
            }
                       
            await _testConductRepository.AddAnswerAsync(attendeeId, JsonConvert.SerializeObject(answer));

            return Ok(answer);
        }

        /// <summary>
        /// Sets the elapsed time from the beginning of Test
        /// </summary>
        /// <param name="attendeeId">Id of Attendee</param>
        [HttpPut("elapsetime/{attendeeId}")]
        public async Task<IActionResult> SetElapsedTimeAsync([FromRoute]int attendeeId)
        {
            if (HttpContext.Session.GetInt32(ATTENDEE_ID).Value != attendeeId)
            {
                return BadRequest();
            }

            await _testConductRepository.SetElapsedTimeAsync(attendeeId);

            return Ok(attendeeId);
        }

        /// <summary>
        /// Gets the elapsed time from the beginning of Test
        /// </summary>
        /// <param name="attendeeId">Id of Attendee</param>
        /// <returns>Time elapsed in double</returns>
        [HttpGet("elapsetime/{attendeeId}")]
        public async Task<IActionResult> GetElapsedTimeAsync([FromRoute]int attendeeId)
        {
            if (HttpContext.Session.GetInt32(ATTENDEE_ID).Value != attendeeId)
            {
                return BadRequest();
            }

            return Ok(await _testConductRepository.GetElapsedTimeAsync(attendeeId));
        }

        /// <summary>
        /// Gets answer from the Database
        /// </summary>
        /// <param name="attendeeId">Id of Test Attendee</param>
        /// <returns>TestAnswerAC object</returns>
        [HttpGet("answer/{attendeeId}")]
        public async Task<IActionResult> GetAnswerAsync([FromRoute]int attendeeId)
        {
            if (HttpContext.Session.GetInt32(ATTENDEE_ID).Value != attendeeId)
            {
                return BadRequest();
            }

            if (!await _testConductRepository.IsTestAttendeeExistByIdAsync(attendeeId))
            {
                return NotFound();
            }

            var attendeeAnswers = await _testConductRepository.GetAnswerAsync(attendeeId);

            if (attendeeAnswers == null)
            {
                return NotFound();
            }

            return Ok(attendeeAnswers);
        }

        /// <summary>
        /// Gets Test Attendee by Id
        /// </summary>
        /// <param name="testId">Id of Test</param>
        /// <returns>TestAttendee object</returns>
        [HttpGet("attendee/{testid}")]
        public async Task<IActionResult> GetTestAttendeeByIdAsync([FromRoute] int testId)
        {
            if(!await _testRepository.IsTestExists(testId))
            {
                return NotFound();
            }

            var attendeeId = HttpContext.Session.GetInt32(ATTENDEE_ID).Value;

            if (!await _testConductRepository.IsTestAttendeeExistByIdAsync(attendeeId))
            {
                return NotFound();
            }

            return Ok(await _testConductRepository.GetTestAttendeeByIdAsync(attendeeId));
        }

        /// <summary>
        /// Gets all the Question in a Test. Answers of the questions are excluded.
        /// </summary>
        /// <param name="id">Id of the Test</param>
        /// <returns>List of QuestionAC object</returns>
        [HttpGet("testquestion/{id}")]
        public async Task<IActionResult> GetTestQuestionByTestIdAsync([FromRoute] int id)
        {
            if (!await _testRepository.IsTestExists(id))
            {
                return NotFound();
            }

            return Ok(await _testRepository.GetTestQuestionByTestIdAsync(id));
        }
        
        /// <summary>
        /// Returns Test by Test Link
        /// </summary>
        /// <param name="link">Link of the Test</param>
        /// <returns>TestAC object</returns>
        [HttpGet("testbylink/{link}")]
        public async Task<IActionResult> GetTestByLinkAsync([FromRoute]string link)
        {
            if (link == null)
            {
                return BadRequest();
            }

            return Ok(await _testRepository.GetTestByLinkAsync(link));
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