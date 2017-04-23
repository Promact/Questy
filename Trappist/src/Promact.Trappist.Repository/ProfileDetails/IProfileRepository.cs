using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.ProfileDetails
{
  public interface IProfileRepository
  {

    /// <summary>
    /// Updates current user's profile details
    /// </summary>
    /// <param name="updateUserDetails">takes parameter of type ApplicationUser which comes from the client side(from body)</param>
    /// <returns>Update user details in database</returns>
    void UpdateProfile(ApplicationUser updateUserDetails);

    /// <summary>
    /// fetch the profile details of the cureent user 
    /// </summary>
    /// <param name="name">takes parameter of string type which has the name of the current user whose details have to be fetched</param>
    /// <returns>fetch the details from the database</returns>
    Task<ApplicationUser> GetUserDetails(string name);

    /// <summary>
    /// changes User's Password
    /// </summary>
    /// <param name="model">takes parameter of type ChangePasswordModel</param>
    /// <returns>save new password of the user in the database</returns>
    Task<ApplicationUser> UpdaetUserPassword(ChangePasswordModel model);

  }
}
