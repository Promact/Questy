using Promact.Trappist.Web.Models;

namespace Promact.Trappist.Repository.ProfileDetails
{
    public interface IProfileRepository
    {

    /// <summary>
    /// Updates current user's details
    /// </summary>
    /// <param name="updateUserDetails"></param>
    /// <returns>Update user details in database</returns>
    void UpdateProfile(ApplicationUser updateUserDetails);
    }
}
