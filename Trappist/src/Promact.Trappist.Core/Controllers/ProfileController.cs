using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

    public ProfileController(IProfileRepository profileRepository, SignInManager<ApplicationUser> signInManager)
    {
      _profileRepository = profileRepository;
      _signInManager = signInManager;
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
