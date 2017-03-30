using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.TestSettings;
using Promact.Trappist.Repository.TestSettings;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetTestSettings([FromRoute] int id)
        {
            var settings = await _settingsRepository.GetTestSettingsAsync(id);
            if (settings == null)
            {
                return NotFound();
            }
            else
                return Ok(settings);
        }

        /// <summary>
        /// Updates the changes made to the settings of a Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to access the Settings of that Test</param>
        /// <param name="testSettingsAC">The parameter "testSettingsAC" is an object of TestSettingsAC</param>
        /// <returns>Updated Settings of that Test</returns>     
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTestSettings([FromRoute] int id, [FromBody] TestSettingsAC testSettingsAC)
        {
            if (await _settingsRepository.TestSettingsExists(id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _settingsRepository.UpdateTestSettingsAsync(testSettingsAC);
                return Ok(testSettingsAC);
            }
            else
                return BadRequest();
        }
    }
}
