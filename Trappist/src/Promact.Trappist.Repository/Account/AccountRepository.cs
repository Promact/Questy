using System;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.ApplicationClasses.Account;
using Microsoft.AspNetCore.Identity;
using Promact.Trappist.Web.Models;
using Promact.Trappist.DomainModel.DbContext;

namespace Promact.Trappist.Repository.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly TrappistDbContext _dbcontext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountRepository(SignInManager<ApplicationUser> signInManager, TrappistDbContext dbcontext)
        {
            _signInManager = signInManager;
            _dbcontext = dbcontext;

        }
        /// <summary>
        /// this method is used to validate user credentials
        /// </summary>
        /// <param name="loginModel">object of Login</param>
        /// <returns>true if matched</returns>
        public async Task<bool> SignIn(Login loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, isPersistent: true, lockoutOnFailure: true);
            return result.Succeeded;
        }

    }
}
