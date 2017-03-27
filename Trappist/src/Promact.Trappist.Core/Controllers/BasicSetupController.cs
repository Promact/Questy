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
        /// <returns>If valid then return true else return false in reponse object</returns>
        [Route("connectionstring")]
        [HttpPost]
        public async Task<IActionResult> ValidateConnectionString([FromBody] ConnectionString model)
        {
            if (ModelState.IsValid)
                return Ok(await _basicSetup.ValidateConnectionString(model));
            return Ok(false);
        }

        /// <summary>
        /// This method used for verifying Email settings
        /// </summary>
        /// <param name="model"></param>
        /// <returns>if valid email settings then return true else false in response object</returns>
        [Route("mailsettings")]
        [HttpPost]
        public async Task<IActionResult> ValidateEmailSettings([FromBody] EmailSettings model)
        {
            if (ModelState.IsValid)
            {
                if (await _basicSetup.ValidateEmailSetting(model))
                    return Ok(true);
                return Ok(false);
            }
            return Ok(false);
        }

        /// <summary>
        /// This method used for creating the user, save setup parameter and initialize database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>if a user created then return true else return false</returns>
        [Route("createuser")]
        [HttpPost]
        public async Task<IActionResult> CreateAdminUser([FromBody] BasicSetupModel model)
        {
            if (ModelState.IsValid)
                return Ok(await _basicSetup.CreateAdminUser(model));
            return Ok(false);
        }
        #endregion
    }
}
