using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Web.Models;

namespace Promact.Trappist.Repository.ProfileDetails
{
    public class ProfileRepository :IProfileRepository
    {
    private readonly TrappistDbContext _dbContext;

    public ProfileRepository(TrappistDbContext dbContext)
    {
      _dbContext = dbContext;
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
