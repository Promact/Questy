using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.Report;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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

        [HttpGet("{id}")]
        public IEnumerable GetAllTestAttendees([FromRoute]int id)
        {
            var testAttendees = _reportRepository.GetAllTestAttendees(id);
            return testAttendees;
        }

        [HttpGet("{id}/testAttendee")]
        public async Task<IActionResult> GetTestAttendeeByIdAsync([FromRoute]int id)
        {
            return Ok(await _reportRepository.GetTestAttendeeByIdAsync(id));
        }

        [HttpGet("{id}/testQuestions")]
        public async Task<IActionResult> GetTestQuestions([FromRoute] int id)
        {
           return Ok(await  _reportRepository.GetTestQuestions(id));
        }
    }
}
