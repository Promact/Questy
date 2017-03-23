using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.Repository.TestSettings;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/settings")]
    public class TestSettingsController : Controller
    {
        private readonly ITestSettingsRepository _settingsRepository;

        public TestSettingsController(ITestSettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Gets the Settings saved for a particular Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to get the Settings of a Test by its Id</param>
        /// <returns>Settings saved for the selected Test</returns>
        [HttpGet("{id}")]
        public IActionResult GetTestSettings([FromRoute] int id)
        {
            var settings = _settingsRepository.GetTestSettings(id);
            return Ok(settings);
        }

        /// <summary>
        /// Updates the changes made to the Settings of a Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to access the Settings of that Test</param>
        /// <param name="testObject">The parameter "testObject" is used as an object for the Model Test</param>
        /// <returns>Updated Settings of that Test</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateTestSettings([FromRoute] int id, [FromBody] Test testObject)
        {
            _settingsRepository.UpdateTestSettings(id, testObject);
            return Ok(testObject);
        }
    }
}
