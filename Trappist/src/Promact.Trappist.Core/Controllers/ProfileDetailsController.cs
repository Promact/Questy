using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.ProfileDetails;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
  [Route("api/Profile")]
  public class ProfileDetailsController : Controller
  {
    private readonly IProfileRepository _profileRepository;
    private UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ProfileDetailsController(IProfileRepository profileRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
      _profileRepository = profileRepository;
      _userManager = userManager;
      _signInManager = signInManager;
    }

    /// <summary>
    /// Gets current user's details
    /// </summary>
    /// <returns>User's details</returns>
    [HttpGet]
    public async Task<IActionResult> GetUserDetails()
    {
      var user = await _userManager.FindByNameAsync(User.Identity.Name);
      return Ok(user);
    }

    /// <summary>
    /// Updates current user's details
    /// </summary>
    /// <param name="name"></param>
    /// <param name="updateUserDetails"></param>
    /// <returns>Update user details in database</returns>
    [HttpPut("{name}")]
    public IActionResult UpdateProfile([FromRoute]string name, [FromBody] ApplicationUser updateUserDetails)
    {
      _profileRepository.UpdateProfile(updateUserDetails);
      return Ok();
    }

  }
}
