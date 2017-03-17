using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.ProfileDetails
{
  public class ProfileRepository :IProfileRepository
    {
    private readonly TrappistDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileRepository(TrappistDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
    }


    public async Task<ApplicationUser> GetUserDetails(string name)
    {
      var user = await _userManager.FindByNameAsync(name);
      return user;
    }

    /// <summary>
    /// Updates current user's details
    /// </summary>
    /// <param name="updateUserDetails"></param>
    /// <returns>Update user details in database</returns>
    public void UpdateProfile(ApplicationUser updateUserDetails)
    {
      var user = _dbContext.Users.Update(updateUserDetails);
      _dbContext.SaveChanges();
    }


  }
}
