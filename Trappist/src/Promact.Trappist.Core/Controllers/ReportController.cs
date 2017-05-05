using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.Report;
using System.Collections;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/reports")]
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportrepository)
        {
            _reportRepository = reportrepository;
        }

        #region Single Student Report API
        /// <summary>
        /// Gets all the test attendees present in the test
        /// </summary>
        /// <param name="id">Id of the test whose attendees are to be fetched</param>
        /// <returns>A list of test attendees present in the test</returns>
        [HttpGet("{id}")]
        public IEnumerable GetAllTestAttendees([FromRoute]int id)
        {
            var testAttendees = _reportRepository.GetAllTestAttendees(id);
            return testAttendees;
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
        #endregion
    }
}
