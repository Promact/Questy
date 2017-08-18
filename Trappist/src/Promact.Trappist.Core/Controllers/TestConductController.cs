using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.DomainModel.Models.TestLogs;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Repository.Tests;
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
        private readonly ITestsRepository _testRepository;
        private readonly IStringConstants _stringConstants;
        #endregion
        #endregion

        #region Constructor
        public TestConductController(ITestConductRepository testConductRepository, ITestsRepository testRepository, IStringConstants stringConstants)
        {
            _testConductRepository = testConductRepository;
            _testRepository = testRepository;
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
                HttpContext.Session.SetInt32(_stringConstants.AttendeeIdSessionKey, testAttendee.Id);
                HttpContext.Session.SetString(_stringConstants.TestLinkSessionKey, magicString);
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
        public async Task<IActionResult> AddAnswerAsync([FromRoute]int attendeeId, [FromBody] TestAnswerAC answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await IsAttendeeValid(attendeeId))
            {
                return NotFound();
            }

            //If the Attendee has completed the Test
            var attendeeTestStatus = await _testConductRepository.GetAttendeeTestStatusAsync(attendeeId);
            if (attendeeTestStatus != TestStatus.AllCandidates)
            {
                return BadRequest();
            }

            if (!await _testConductRepository.IsAnswerValidAsync(attendeeId, answer))
            {
                return BadRequest();
            }

            await _testConductRepository.AddAnswerAsync(attendeeId, answer);

            return Ok(answer);
        }

        /// <summary>
        /// Sets the elapsed time from the beginning of Test
        /// </summary>
        /// <param name="attendeeId">Id of Attendee</param>
        [HttpPut("elapsetime/{attendeeId}")]
        public async Task<IActionResult> SetElapsedTimeAsync([FromRoute]int attendeeId)
        {
            if (!await IsAttendeeValid(attendeeId))
            {
                return NotFound();
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
            if (!await IsAttendeeValid(attendeeId))
            {
                return NotFound();
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
            if (!await IsAttendeeValid(attendeeId))
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
        [HttpGet("attendee/{testid}/{isPreview}")]
        public async Task<IActionResult> GetTestAttendeeByIdAsync([FromRoute] int testId, [FromRoute] bool isPreview)
        {
            if (!await _testRepository.IsTestExists(testId))
            {
                return NotFound();
            }

            if (HttpContext.Session.GetInt32(_stringConstants.AttendeeIdSessionKey) == null && !isPreview)
            {
                return NotFound();
            }

            var attendeeId = HttpContext.Session.GetInt32(_stringConstants.AttendeeIdSessionKey).Value;

            if (!await IsAttendeeValid(attendeeId))
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
        [HttpGet("testbylink/{link}/{isPreview}")]
        public async Task<IActionResult> GetTestByLinkAsync([FromRoute]string link, [FromRoute] bool isPreview)
        {
            if (link == null)
            {
                return BadRequest();
            }

            if (!await _testConductRepository.IsTestInValidDateWindow(link, isPreview))
            {
                return NotFound();
            }

            return Ok(await _testRepository.GetTestByLinkAsync(link));
        }

        /// <summary>
        /// Sets the TestStatus of Attendee
        /// </summary>
        /// <param name="attendeeId">Id of Attendee</param>
        /// <param name="testStatus">TestStatus object</param>
        [HttpPut("teststatus/{attendeeId}")]
        public async Task<IActionResult> SetTestStatusAsync([FromRoute]int attendeeId, [FromBody]TestStatus testStatus)
        {
            if (!await IsAttendeeValid(attendeeId))
            {
                return NotFound();
            }

            //If the Attendee has completed the Test
            var attendeeTestStatus = await _testConductRepository.GetAttendeeTestStatusAsync(attendeeId);
            if (attendeeTestStatus != TestStatus.AllCandidates)
            {
                return BadRequest();
            }

            await _testConductRepository.SetAttendeeTestStatusAsync(attendeeId, testStatus);

            return Ok(attendeeId);
        }

        /// <summary>
        /// Gets TestStatus of Attendee
        /// </summary>
        /// <param name="attendeeId">Id of Attendee</param>
        /// <returns>TestStatus object</returns>
        [HttpGet("teststatus/{attendeeId}")]
        public async Task<IActionResult> GetTestStatusAsync([FromRoute]int attendeeId)
        {
            if (!await IsAttendeeValid(attendeeId))
            {
                return NotFound();
            }

            return Ok(await _testConductRepository.GetAttendeeTestStatusAsync(attendeeId));
        }

        [HttpPost("code/{attendeeId}")]
        public async Task<IActionResult> EvaluateCodeSnippet([FromRoute]int attendeeId, [FromBody]TestAnswerAC testAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (testAnswer.Code == null || testAnswer.Code.Source == "")
            {
                return BadRequest();
            }
            var codeResponse = await _testConductRepository.ExecuteCodeSnippetAsync(attendeeId, testAnswer);

            return Ok(codeResponse);
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

        #region Test-Summary API
        /// <summary>
        /// Returns the total number of questions in a particular test
        /// </summary>
        /// <param name="testLink">Contains the link of the Test</param>
        /// <returns></returns>
        [HttpGet("{testLink}/test-summary")]
        public async Task<int> GetTestSummaryAsync(string testLink)
        {
            var summaryDetails = await _testConductRepository.GetTestSummaryDetailsAsync(testLink);
            return summaryDetails;
        }
        #endregion
        #endregion

        #region Private Method
        private async Task<bool> IsAttendeeValid(int attendeeId)
        {
            if (HttpContext.Session.GetInt32(_stringConstants.AttendeeIdSessionKey) == null)
            {
                return false;
            }

            if (HttpContext.Session.GetInt32(_stringConstants.AttendeeIdSessionKey).Value == attendeeId
                && await _testConductRepository.IsTestAttendeeExistByIdAsync(attendeeId))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the values of certain fields of TestLogs Method
        /// </summary>
        /// <param name="attendeeId">Id of a particular Test Attendee obtained from route</param>
        /// <param name="testLogs">It is an object of Test Logs type</param>
        /// <returns></returns>
        [HttpPut("testlogs/{attendeeId}")]
        public async Task SetTestLogsAsync([FromRoute]int attendeeId, [FromBody]TestLogs testLogs)
        {
            await _testConductRepository.AddTestLogsAsync(attendeeId, testLogs);
        }

        [HttpGet("testlogs")]
        public async Task<IActionResult> GetTestLogs()
        {
            return Ok(await _testConductRepository.GetTestLogsAsync());

        }
        #endregion
    }
}