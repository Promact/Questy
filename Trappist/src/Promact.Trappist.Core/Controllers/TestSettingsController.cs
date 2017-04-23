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

        /// <summary>
        /// Get the Settings saved for a particular Test
        /// </summary>
        /// <returns>Settings set for that Test</returns>
        [HttpGet]
        public IActionResult GetTestSettings()
        {
            var settings = _settingsRepository.GetTestSettings();
            return Json(settings);
        }

        /// <summary>
        /// Updates the changes made to the Settings of a Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to access the Settings of that Test</param>
        /// <param name="testObject">The parameter "testObject" is used as an object for the Model Test</param>
        /// <returns>Updated Settings of that Test</returns>
        [HttpPut("api/put/{id}")]
        public IActionResult UpdateTestSettings([FromRoute] int id, [FromBody] Test testObject)
        {
            _settingsRepository.UpdateTestSettings(id, testObject);
            return Json(testObject);
        }
    }
}
