using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models;
using System;

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
        /// <returns>details of the current user</returns>
        public async Task<ApplicationUser> GetUserDetailsAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return user;
        }

        /// <summary>
        /// Updates current user details
        /// </summary>
        /// <param name="updateUserDetails">parameter of type ApplicationUser which has the updated details of the user profile</param>
        /// <returns>Update user profile details in database</returns>
        public void UpdateUserProfile(ApplicationUser updateUserDetails)
        {
            var user = _dbContext.Users.Update(updateUserDetails);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Update user password 
        /// </summary>
        /// <param name="model">take parameter of type ChangePasswordModel which has the new password of the user</param>
        /// <returns>save new password in the database</returns>
        public async Task<ApplicationUser> UpdateUserPasswordAsync(ChangePasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    _dbContext.SaveChanges();
                    return user;
                }
            }
            throw new NotImplementedException();
        }
    }
}
