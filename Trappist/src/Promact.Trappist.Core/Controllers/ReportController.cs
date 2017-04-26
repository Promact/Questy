using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.Reports;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/report")]
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;

        public  ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("multiple/{id}")]
        public async Task<IActionResult> getAllTestAttendeesAsync([FromRoute] int id)
        {
            return Ok(await _reportRepository.GetAllTestAttendeesAsync(id));
        }
    }
}
