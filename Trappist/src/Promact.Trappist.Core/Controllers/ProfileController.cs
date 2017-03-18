using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.ProfileDetails;
using Promact.Trappist.Web.Models;
using System;
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
    [Route("logOut")]
    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
      await _signInManager.SignOutAsync();
      Console.Write(User);
      return Ok();
    }

  }
}
