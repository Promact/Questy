using System.Threading.Tasks;

namespace Promact.Trappist.Utility.EmailServices
{
    public interface IEmailService
    {
        /// <summary>
        /// This method used for sending mail
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        /// <returns>return true if mail sent sucessfully else return false</returns>
        Task<bool> SendMailAsync(string to, string from, string body, string subject);
    }
}
