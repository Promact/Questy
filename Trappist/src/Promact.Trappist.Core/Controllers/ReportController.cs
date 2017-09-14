using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Report;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.Reports;
using Promact.Trappist.Utility.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/report")]
    public class ReportController : Controller
    {
        #region Private Member
        private readonly IReportRepository _reportRepository;
        private readonly IStringConstants _stringConstants;
        #endregion

        #region Constructor
        public ReportController(IReportRepository reportRepository, IStringConstants stringConstants)
        {
            _reportRepository = reportRepository;
            _stringConstants = stringConstants;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Method to get the details of all the Test-Attendees of the respective test
        /// </summary>
        /// <param name="testId">Id of the respective test</param>
        /// <returns>All test attendees of that respective test</returns>
        [HttpGet("{testId}")]
        public async Task<IActionResult> GetAllTestAttendeesAsync([FromRoute] int testId)
        {
            return Ok(await _reportRepository.GetAllTestAttendeesAsync(testId));
        }

        /// <summary>
        /// Method to set a candidate as Starred candidate
        /// </summary>
        /// <param name="id">Id of the candidate</param>
        /// <returns>Attendee Id</returns>
        [HttpPost("star/{attendeeId}")]
        public async Task<IActionResult> SetStarredCandidateAsync([FromBody] int attendeeId)
        {
            if (!await _reportRepository.IsCandidateExistAsync(attendeeId))
                return NotFound();

            await _reportRepository.SetStarredCandidateAsync(attendeeId);
            return Ok(attendeeId);
        }

        /// <summary>
        /// Method to get test name
        /// </summary>
        /// <param name="Id">Id of the test</param>
        /// <returns>Test name</returns>
        [HttpGet("testName/{Id}")]
        public async Task<IActionResult> GetTestName([FromRoute] int Id)
        {
            return Ok(await _reportRepository.GetTestNameAsync(Id));
        }

        /// <summary>
        /// Method to set all candidates with matching criteria as starred candidate
        /// </summary>
        /// <param name="status">Star status of the candidates</param>
        /// <param name="selectedTestStatus">Test end status of the candidates</param>
        /// <param name="searchString">The search string provided by the user</param>
        /// <returns>Star status of the candidates</returns>
        [HttpPut("star/all/{selectedTestStatus}")]
        public async Task<IActionResult> SetAllCandidateStarredAsync([FromBody] bool status, [FromRoute] int selectedTestStatus, [FromQuery]string searchString)
        {
            await _reportRepository.SetAllCandidateStarredAsync(status, selectedTestStatus, searchString);
            return Ok(status);
        }

        /// <summary>
        /// Gets the details of the test attendee along with his marks and test logs
        /// </summary>
        /// <param name="id">Id of the test attendee</param>
        /// <returns> An object of test attendee</returns>
        [HttpGet("{id}/testAttendee")]
        public async Task<IActionResult> GetTestAttendeeDetailsByIdAsync([FromRoute]int id)
        {
            return Ok(await _reportRepository.GetTestAttendeeDetailsByIdAsync(id));
        }

        /// <summary>
        /// Gets all the questions present in a test
        /// </summary>
        /// <param name="id">Id of the test qhose questions are to be fetched</param>
        /// <returns> A list of questions</returns>
        [HttpGet("{id}/testQuestions")]
        public async Task<IActionResult> GetTestQuestions([FromRoute] int id)
        {
            return Ok(await _reportRepository.GetTestQuestions(id));
        }

        /// <summary>
        /// Gets all the answers given by the test attendee
        /// </summary>
        /// <param name="id">Id of the test attendee</param>
        /// <returns>A list of answers given by the test attendee</returns>
        [HttpGet("{id}/testAnswers")]
        public async Task<IActionResult> GetTestAttendeeAnswers([FromRoute] int id)
        {
            return Ok(await _reportRepository.GetTestAttendeeAnswers(id));
        }

        /// <summary>
        /// Gets the percentile of the selected test attendee
        /// </summary>
        /// <param name="attendeeId">Id of the test attendee</param>
        /// <param name="testId">Id of the test taken by the test attendee</param>
        /// <returns>The percentile calculated for the selected candidate</returns>
        [HttpGet("{attendeeId}/{testId}/percentile")]
        public async Task<IActionResult> GetStudentPercentile([FromRoute]int attendeeId,[FromRoute]int testId)
        {
            return Ok(await _reportRepository.CalculatePercentileAsync(attendeeId,testId));
        }

        /// <summary>
        /// Gets the marks details of all attendee 
        /// </summary>
        /// <param name="testId">Id of a test</param>
        /// <returns></returns>
        [HttpGet("{testId}/allAttendeeMarksDeatils")]
        public async Task<IActionResult> GetAllAttendeeMarksDetails([FromRoute] int testId)
        {
            return Ok(await _reportRepository.GetAllAttendeeMarksDetailsAsync(testId));
        }
        /// <summary>
        /// Creates session for the candidate to resume the test
        /// </summary>
        /// <param name="attendee"></param>
        /// <param name="testLink"></param>
        /// <returns></returns>
        [HttpPost("createSession/{testLink}/{isTestEnd}")]
        public async Task<IActionResult> CreateSessionForAttendee([FromBody] TestAttendees attendee, [FromRoute] string testLink,[FromRoute] bool isTestEnd)
        {
            if (isTestEnd)
            {
               var response = await _reportRepository.SetTestStatusAsync(attendee, isTestEnd);
                if (response == null)
                    return NotFound();
            }
            else
            {
                if (HttpContext.Session.GetInt32(_stringConstants.AttendeeIdSessionKey) == null)
                    HttpContext.Session.SetInt32(_stringConstants.AttendeeIdSessionKey, attendee.Id);
                var response = await _reportRepository.SetTestStatusAsync(attendee, isTestEnd);
                if (response == null)
                    return NotFound();
            }
            return Ok(attendee);           
        }

        /// <summary>
        /// Gets the details of Code Snippet Question Test Cases
        /// </summary>
        /// <param name="attendeeId">It contains the id of the test attendee from the route</param>
        /// <param name="questionId">It contains the question id of code snippet question of a particular test</param>
        /// <returns>List of details of each test case for code snippet question attended by test attendee</returns>
        [HttpGet("{attendeeId}/{questionId}/codeSnippetTestCasesDetails")]
        public async Task<IActionResult> GetCodeSnippetTestCasesDetailsAsync([FromRoute] int attendeeId,int questionId)
        {
            return Ok(await _reportRepository.GetCodeSnippetDetailsAsync(attendeeId,questionId));
        }

        /// <summary>
        /// Gets the total marks scored by the test attendee in code snippet question
        /// </summary>
        /// <param name="attendeeId">It contains the id of the test attendee from the route</param>
        /// <param name="questionId">It contains the question id of code snippet question of a particular test</param>
        /// <returns>The total marks obtained by test attendee in code snippet question</returns>
        [HttpGet("{attendeeId}/{questionId}/scoreOfCodeSnippetQuestion")]
        public async Task<IActionResult> GetCodeSnippetQuestionMarksAsync([FromRoute]int attendeeId,int questionId)
        {
            return Ok(await _reportRepository.GetTotalMarksOfCodeSnippetQuestionAsync(attendeeId,questionId));
        }

        /// <summary>
        /// Gets the test code solution details of the test cases of code snippet questions attended by a test attendee
        /// </summary>
        /// <param name="attendeeId">It contains the id of the test attendee from the route</param>
        /// <param name="questionId">It contains the question id of code snippet question of a particular test</param>
        /// <returns>The test code solution details of the code snippet question attended by test attendees</returns>
        [HttpGet("{attendeeId}/{questionId}/testCodeSolutionDetails")]
        public async Task<IActionResult> GetTestCodeSolutionDetailsAsync([FromRoute]int attendeeId,int questionId)
        {
            return Ok(await _reportRepository.GetTestCodeSolutionDetailsAsync(attendeeId,questionId));
        }

        /// <summary>
        /// Sends request to the conductor for test resume
        /// </summary>
        /// <param name="attendeeId"></param>
        /// <param name="isTestResume"></param>
        /// <returns></returns>
        [HttpGet("{attendeeId}/{isTestResume}/sendRequest")]
        public async Task<bool> SendRequestToResumeTestAsync([FromRoute] int attendeeId, [FromRoute] bool isTestResume)
        {
            await _reportRepository.SetWindowCloseAsync(attendeeId, isTestResume);
            return true;
        }

        /// <summary>
        /// Gets the status of test if it is resumed
        /// </summary>
        /// <param name="attendeeId"></param>
        /// <returns></returns>
        [HttpGet("getWindowClose/{attendeeId}")]
        public async Task<IActionResult> GetResumeTestValueAsync([FromRoute] int attendeeId)
        {
            return Ok(await _reportRepository.GetWindowCloseAsync(attendeeId));
        }

        /// <summary>
        /// Gets the number of questions attempted by a test attendee
        /// </summary>
        /// <param name="attendeeId">Contains the value of the attendee Id from the route</param>
        /// <returns>Number of questions attempted by an attendee</returns>
        [HttpGet("{attendeeId}/attemptedQuestions")]
        public async Task<int> GetTotalNumberOfAttemptedQuestionsByAttendee([FromRoute]int attendeeId)
        {
            return await _reportRepository.GetAttemptedQuestionsByAttendeeAsync(attendeeId);
        }

        [HttpPost("generateReport")]
        public async Task<IActionResult> GenerateReportForUnfinishedTestAsync([FromBody]List<int> attendeeIdList)
        {
            return Ok(await _reportRepository.GenerateReportForUnfinishedTestAsync(attendeeIdList));
        }
        #endregion
    }
}