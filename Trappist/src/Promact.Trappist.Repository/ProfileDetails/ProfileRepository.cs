using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.ProfileDetails
{
  public class ProfileRepository : IProfileRepository
  {
    private readonly TrappistDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileRepository(TrappistDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
    }

    /// <summary>
    /// fetch the profile details of the cureent user 
    /// </summary>
    /// <param name="name">takes parameter of string type which has the name of the current user whose details have to be fetched</param>
    /// <returns>fetch the details from the database and return those values</returns>
    public async Task<ApplicationUser> GetUserDetails(string name)
    {
      var user = await _userManager.FindByNameAsync(name);
      return user;
    }

    /// <summary>
    /// Updates current user's details
    /// </summary>
    /// <param name="updateUserDetails"></param>
    /// <returns>Update user's profile details in database</returns>
    public void UpdateProfile(ApplicationUser updateUserDetails)
    {
      var user = _dbContext.Users.Update(updateUserDetails);
      _dbContext.SaveChanges();
    }
  }
}
