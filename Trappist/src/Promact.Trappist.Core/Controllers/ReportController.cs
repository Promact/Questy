using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.Reports;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/report")]
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

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

        [HttpPut("star/all/{testId}")]
        public async Task<IActionResult> SetAllCandidateStarredAsync([FromRoute] int testId, [FromBody] bool status)
        {
            await _reportRepository.SetAllCandidateStarredAsync(testId, status);
            return Ok(testId);
        }
    }
}
