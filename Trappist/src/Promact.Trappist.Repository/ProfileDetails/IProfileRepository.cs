﻿using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.ProfileDetails
{
    public interface IProfileRepository
    {
        /// <summary>
        /// Update current user profile details
        /// </summary>
        /// <param name="updateUserDetails">takes parameter of type ApplicationUser</param>
        /// <returns>Updated user details in database</returns>
        void UpdateProfile(ApplicationUser updateUserDetails);

        /// <summary>
        /// fetch the profile details of the cureent user 
        /// </summary>
        /// <param name="name">takes parameter of string type which has the name of the current user whose details have to be fetched</param>
        /// <returns>fetch the details from the database</returns>
        Task<ApplicationUser> GetUserDetails(string name);

        /// <summary>
        /// changes User Password
        /// </summary>
        /// <param name="model">takes parameter of type ChangePasswordModel</param>
        /// <returns>save new password of the user in the database</returns>
        Task<ApplicationUser> UpdateUserPassword(ChangePasswordModel model);
    }
}
