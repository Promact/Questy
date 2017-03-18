using System;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models;
using Microsoft.AspNetCore.Identity;
using Promact.Trappist.Web.Models;
using Promact.Trappist.DomainModel.DbContext;

namespace Promact.Trappist.Repository.ChangePassword
{
  public class PasswordRepository : IPasswordRepository
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TrappistDbContext _trappistDbContext;

    public PasswordRepository(UserManager<ApplicationUser> userManager, TrappistDbContext trappistDbContext)
    {
      _userManager = userManager;
      _trappistDbContext = trappistDbContext;
    }

    /// <summary>
    /// Update user password 
    /// </summary>
    /// <param name="model">take parameter of type ChangePasswordModel</param>
    /// <returns> save new password in the database</returns>
    public async Task<ApplicationUser> UpdaetUserPassword(ChangePasswordModel model)
    {
      var user = await _userManager.FindByEmailAsync(model.Email);

      if (user != null)
      {
        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (result.Succeeded)
        {
          _trappistDbContext.SaveChanges();
        }
      }

      throw new NotImplementedException();
    }
  }
}
