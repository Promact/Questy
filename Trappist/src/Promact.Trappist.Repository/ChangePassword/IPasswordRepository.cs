using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.Web.Models;

namespace Promact.Trappist.Repository.ChangePassword
{
  public interface IPasswordRepository
  {
    /// <summary>
    /// changes User's Password
    /// </summary>
    /// <param name="model">takes parameter of type ChangePasswordModel</param>
    /// <returns>save new password of the user in the database</returns>
    Task<ApplicationUser> UpdaetUserPassword(ChangePasswordModel model);
  }
}
