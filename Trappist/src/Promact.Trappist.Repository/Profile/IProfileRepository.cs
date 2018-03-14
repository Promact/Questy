using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Profile
{
    public interface IProfileRepository
    {
        /// <summary>
        /// Update current user profile details
        /// </summary>
        /// <param name="updateUserDetails">Takes parameter of type ApplicationUser</param>
        /// <returns>Updated user details in database</returns>
        void UpdateUserProfile(ApplicationUser updateUserDetails);

        /// <summary>
        /// Fetch the profile details of the cureent user 
        /// </summary>
        /// <param name="name">Takes parameter of string type which has the name of the current user whose details have to be fetched</param>
        /// <returns>Fetch the details from the database</returns>
        Task<ApplicationUser> GetUserDetailsAsync(string name);

        /// <summary>
        /// changes User Password
        /// </summary>
        /// <param name="name">Stores the name of the current user</param>
        /// <param name="model">Take parameter of type ChangePasswordModel which has the new password of the user</param>
        /// <returns>Save new password of the user in the database</returns>
        Task<bool> UpdateUserPasswordAsync(string name,ChangePasswordModel model);
    }
}