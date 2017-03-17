using Microsoft.AspNetCore.Authorization;
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

    public ProfileController(IProfileRepository profileRepository)
    {
      _profileRepository = profileRepository;
    }

    /// <summary>
    /// Gets current user's details
    /// </summary>
    /// <returns>User's details</returns>
    [HttpGet]
    public async Task<IActionResult> GetUserDetails()
    {
      var user = await _profileRepository.GetUserDetails(User.Identity.Name);
      return Ok(user);
    }

    /// <summary>
    /// Updates current user's details
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

  }
}
