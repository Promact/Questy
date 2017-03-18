using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.Repository.ChangePassword;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
  [Route("api/changePassword")]
  public class PasswordController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPasswordRepository _passwordRepository;

    public PasswordController(UserManager<ApplicationUser> userManager, IPasswordRepository passwordRepository)
    {
      _userManager = userManager;
      _passwordRepository = passwordRepository;
    }

    /// <summary>
    /// Update user password 
    /// </summary>
    /// <param name="model">takes the parameter of type ChangePasswordModel</param>
    /// <returns>call UpdaetUserPassword() to save the new password in the database</returns>
    [HttpPut]
    public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      var user = await _userManager.GetUserAsync(User);
      model.Email = user.Email;
      return Ok(await _passwordRepository.UpdaetUserPassword(model));

    }
  }
}
