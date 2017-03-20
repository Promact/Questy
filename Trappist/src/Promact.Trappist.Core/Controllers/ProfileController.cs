using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.Repository.ProfileDetails;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
  [Authorize]
  [Route("api/profile")]
  public class ProfileController : Controller
  {
    private readonly IProfileRepository _profileRepository;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(IProfileRepository profileRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
      _profileRepository = profileRepository;
      _signInManager = signInManager;
      _userManager = userManager;
    }

    /// <summary>
    /// Gets current user's profile details
    /// </summary>
    /// <returns>User's profile details</returns>
    [HttpGet]
    public async Task<IActionResult> GetUserDetails()
    {
      if (!User.Identity.IsAuthenticated)
      {
        Response.Redirect("login");
      }
      var user = await _profileRepository.GetUserDetails(User.Identity.Name);
      return Ok(user);
    }

    /// <summary>
    /// Update current user's profile details
    /// </summary>
    /// <param name="updateUserDetails">takes parameter of type ApplicationUser which comes from the client side(from body)</param>
    /// <returns>Update user's profile details in database</returns>

    [HttpPut]
    public IActionResult UpdateProfile([FromBody]ApplicationUser updateUserDetails)
    {
      if (ModelState.IsValid)
      {
        _profileRepository.UpdateProfile(updateUserDetails);
        return Ok();
      }
      return BadRequest();
    }

    /// <summary>
    /// Update user password 
    /// </summary>
    /// <param name="model">takes the parameter of type ChangePasswordModel</param>
    /// <returns>call UpdaetUserPassword() to save the new password in the database</returns>
    [Route("password")]
    [HttpPut]
    public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var user = await _userManager.GetUserAsync(User);
      model.Email = user.Email;
      return Ok(await _profileRepository.UpdaetUserPassword(model));

    }


    /// <summary>
    /// user logeed out
    /// </summary>
    /// <returns> user is logged out and the user's session is ended and is redirected to login page</returns>

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction(nameof(AccountController.Login), "Account");
    }

  }
}
