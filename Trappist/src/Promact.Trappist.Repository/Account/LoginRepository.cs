using Microsoft.AspNetCore.Identity;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;
using System;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.ApplicationClasses.Account;

namespace Promact.Trappist.Repository.Account
{
    public class LoginRepository : ILoginRepository
    {
        private readonly TrappistDbContext _dbcontext;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginRepository(SignInManager<ApplicationUser> signInManager, TrappistDbContext dbcontext)
        {
            _signInManager = signInManager;
            _dbcontext = dbcontext;

        }
        /// <summary>
        /// this method is used to check email and password with database
        /// </summary>
        /// <param name="loginModel">object of Login</param>
        /// <returns>true if matched</returns>
        public async Task<bool> LoginUser(Login loginModel)
        {
            try
            {
                var search = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, isPersistent: true, lockoutOnFailure: true);
                if (search.Succeeded)
                {
                    return true;
                }
                else
                {                   
                    return false;
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}



