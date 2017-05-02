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
        [HttpGet("multiple/{testId}")]
        public async Task<IActionResult> GetAllTestAttendeesAsync([FromRoute] int testId)
        {
            return Ok(await _reportRepository.GetAllTestAttendeesAsync(testId));
        }

        [HttpPost("star/{attendeeId}")]
        public async Task<IActionResult> SetStarredCandidateAsync([FromBody] int attendeeId)
        {
            await _reportRepository.SetStarredCandidateAsync(attendeeId);
            return Ok(attendeeId);
        }

        [HttpPut("star/all/{testId}/{status}")]
        public async Task<IActionResult> SetAllCandidateStarredAsync([FromRoute] int testId, [FromRoute] bool status, [FromBody] List<int> idList)
        {
            await _reportRepository.SetAllCandidateStarredAsync(testId, status, idList);
            return Ok(testId);
        }
        #endregion
    }
}
