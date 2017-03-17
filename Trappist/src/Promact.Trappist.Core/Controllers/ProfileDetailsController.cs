using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.ProfileDetails;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
  [Route("api/profile")]
  public class ProfileDetailsController : Controller
  {
    private readonly IProfileRepository _profileRepository;

    public ProfileDetailsController(IProfileRepository profileRepository)
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
    /// <param name="name"></param>
    /// <param name="updateUserDetails">takes parameter of type ApplicationUser which comes from the client side(from body)</param>
    /// <returns>Update user details in database</returns>
    [HttpPut]
    public IActionResult UpdateProfile([FromBody]ApplicationUser updateUserDetails)
    {
      _profileRepository.UpdateProfile(updateUserDetails);
      return Ok();
    }

  }
}
