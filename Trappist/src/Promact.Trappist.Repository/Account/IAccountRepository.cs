using Promact.Trappist.DomainModel.ApplicationClasses.Account;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Account
{
    public interface IAccountRepository
    {
        /// <summary>
        /// this method is used to signin user with correct given credentials
        /// </summary>
        /// <param name="loginModel">object of login model</param>
        /// <returns></returns>
        Task<bool> SignIn(Login loginModel);
    }
}
