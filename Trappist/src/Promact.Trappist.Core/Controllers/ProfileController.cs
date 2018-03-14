using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.Repository.Profile;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Authorize]
    [Route("api/profile")]
    public class ProfileController : Controller
    {
        #region Private Variables
        private readonly IProfileRepository _profileRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringConstants _stringConstant;
        #endregion

        #region Constructor
        public ProfileController(IProfileRepository profileRepository, SignInManager<ApplicationUser> signInManager, IStringConstants stringConstant)
        {
            _profileRepository = profileRepository;
            _signInManager = signInManager;
            _stringConstant = stringConstant;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets current user profile details
        /// </summary>
        /// <returns>User profile details</returns>
        [HttpGet]
        public async Task<IActionResult> GetUserDetails()
        {
            var user = await _profileRepository.GetUserDetailsAsync(User.Identity.Name);
            return Ok(user);
        }

        /// <summary>
        /// Update current user profile details
        /// </summary>
        /// <param name="updateUserDetails">takes parameter of type ApplicationUser which comes from the client side(from body)</param>
        /// <returns>Update user profile details in database</returns>
        [HttpPut]
        public IActionResult UpdateProfile([FromBody]ApplicationUser updateUserDetails)
        {
            if (ModelState.IsValid)
            {
                _profileRepository.UpdateUserProfile(updateUserDetails);
                return Ok(updateUserDetails);
            }
            return BadRequest();
        }

        /// <summary>
        /// Update user password 
        /// </summary>
        /// <param name="model">takes the parameter of type ChangePasswordModel</param>
        /// <returns>save the new password in the database</returns>
        [Route("password")]
        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", _stringConstant.InvalidPasswordFormatError);
                return BadRequest(ModelState);
            }
            var result = await _profileRepository.UpdateUserPasswordAsync(User.Identity.Name, model);
            if (!result)
            {
                ModelState.AddModelError("error", _stringConstant.InvalidOldPasswordError);
                return BadRequest(ModelState);
            }
            return Ok(result);
        }

        /// <summary>
        /// user logeed out
        /// </summary>
        /// <returns> user is logged out and is redirected to login page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }
        #endregion
    }
}