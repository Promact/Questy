using Promact.Trappist.DomainModel.ApplicationClasses.Account;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Account
{
    /// <summary>
    /// inteface for login user
    /// </summary>
    public interface ILoginRepository
    {
        Task<bool> LoginUser(Login loginModel);
    }
}
