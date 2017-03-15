using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.Repository.TestSettings;

namespace Promact.Trappist.Core.Controllers
{
    public class TestSettingsController : Controller
    {
        private readonly ITestSettingsRepository _settingsRepository;

        public TestSettingsController(ITestSettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        [HttpGet]
        public IActionResult GetTestSettings()
        {
            var settings = _settingsRepository.GetTestSettings();
            return Json(settings);
        }

        [HttpPut("api/put/{id}")]
        public IActionResult UpdateTestSettings([FromRoute] int id, [FromBody] Test testObject)
        {
            _settingsRepository.UpdateTestSettings(id, testObject);
            return Json(testObject);
        }
    }
}
