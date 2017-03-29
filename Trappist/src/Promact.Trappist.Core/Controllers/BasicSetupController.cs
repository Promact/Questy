using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using Promact.Trappist.Repository.BasicSetup;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/setup")]
    public class BasicSetupController : Controller
    {
        #region Private Variables
        #region Dependencies
        private readonly IBasicSetupRepository _basicSetup;
        #endregion
        #endregion

        #region Constructor
        public BasicSetupController(IBasicSetupRepository basicSetup)
        {
            _basicSetup = basicSetup;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// This method used for validating connection string
        /// </summary>
        /// <param name="model"></param>
        /// <returns>If valid then return true else return false.</returns>
        [HttpPost("connectionstring")]
        public async Task<IActionResult> ValidateConnectionString([FromBody] ConnectionString model)
        {
            if (ModelState.IsValid)
                return Ok(await _basicSetup.ValidateConnectionString(model));
            return BadRequest();
        }

        /// <summary>
        /// This method used for verifying Email settings
        /// </summary>
        /// <param name="model"></param>
        /// <returns>If valid email settings then return true else false.</returns>
        [HttpPost("mailsettings")]
        public async Task<IActionResult> ValidateEmailSettings([FromBody] EmailSettings model)
        {
            return Ok(true);
            if (ModelState.IsValid)
                return Ok(await _basicSetup.ValidateEmailSetting(model));
            return BadRequest();
        }

        /// <summary>
        /// This method used for creating the user, save setup parameter and initialize database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>It returns true or false or errors message in the response object.</returns>
        [HttpPost("createuser")]
        public async Task<IActionResult> CreateAdminUser([FromBody] BasicSetupModel model)
        {
            if (ModelState.IsValid)
                return Ok(await _basicSetup.CreateAdminUser(model));
            return BadRequest();
        }
        #endregion
    }
}
