using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.Reports;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/report")]
    public class ReportController : Controller
    {
        #region Private Member
        private readonly IReportRepository _reportRepository;
        #endregion

        #region Constructor
        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        #endregion

        #region Report API

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
        /// <returns>A listy of answers given by the test attendee</returns>
        [HttpGet("{id}/testAnswers")]
        public async Task<IActionResult> GetTestAttendeeAnswers([FromRoute] int id)
        {
            return Ok(await _reportRepository.GetTestAttendeeAnswers(id));
        }

        /// <summary>
        /// Gets the percentile of the selected test attendee
        /// </summary>
        /// <param name="attendeeId">Id of the test attendee</param>
        /// <returns>The percentile calculated for the selected candidate</returns>
        [HttpGet("{attendeeId}/percentile")]
        public async Task<IActionResult> GetStudentPercentile([FromRoute]int attendeeId)
        {
            return Ok(await _reportRepository.CalculatePercentileAsync(attendeeId));
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

        [HttpGet("{attendeeId}/codeSnippetTestCasesDetails")]
        public async Task<IActionResult> GetCodeSnippetTestCasesDetailsAsync([FromRoute] int attendeeId)
        {
            return Ok(await _reportRepository.GetCodeSnippetDetailsAsync(attendeeId));
        }
        #endregion
    }
}
