using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.Reports;
using System.Collections.Generic;
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
        /// <returns></returns>
        [HttpGet("{testId}")]
        public async Task<IActionResult> GetAllTestAttendeesAsync([FromRoute] int testId)
        {
            return Ok(await _reportRepository.GetAllTestAttendeesAsync(testId));
        }

        /// <summary>
        /// Method to set a candidate as Starred candidate
        /// </summary>
        /// <param name="id">Id of the candidate</param>
        /// <returns></returns>
        [HttpPost("star/{attendeeId}")]
        public async Task<IActionResult> SetStarredCandidateAsync([FromBody] int attendeeId)
        {
            await _reportRepository.SetStarredCandidateAsync(attendeeId);
            return Ok(attendeeId);
        }

        /// <summary>
        /// Method to set a list of candidates as starred candidate.
        /// </summary>
        /// <param name="id">Id if the test</param>
        /// <param name="status">Star status of the student</param>
        /// <param name="idList">List of id of test attendees</param>
        /// <returns></returns>
        [HttpPut("star/all/{selectedTestStatus}/{search}")]
        public async Task<IActionResult> SetAllCandidateStarredAsync([FromBody] bool status, [FromRoute] int selectedTestStatus,[FromQuery] string searchString)
        {
            await _reportRepository.SetAllCandidateStarredAsync(status, selectedTestStatus, searchString);
            return Ok(status);
        }
        #endregion
    }
}
