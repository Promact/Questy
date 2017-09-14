using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models;
using System;

namespace Promact.Trappist.Repository.Profile
{
    public class ProfileRepository : IProfileRepository
    {
        #region Private Variables
        private readonly TrappistDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructor
        public ProfileRepository(TrappistDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        #endregion

        #region IProfileRepository Methods
        #region Public Methods
        public async Task<ApplicationUser> GetUserDetailsAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return user;
        }

        public void UpdateUserProfile(ApplicationUser updateUserDetails)
        {
            _dbContext.Users.Update(updateUserDetails);
            _dbContext.SaveChanges();
        }

        public async Task<bool> UpdateUserPasswordAsync(string name, ChangePasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(name);
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        #endregion
        #endregion
    }
}