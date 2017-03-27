using System.Threading.Tasks;

namespace Promact.Trappist.Utility.EmailServices
{
    public interface IEmailService
    {
        /// <summary>
        /// This method used for sending mail to client
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="body"></param>
        /// <param name="to"></param>
        /// <returns>It return true if mail sent else return false.</returns>
        Task<bool> SendMailAsync(string userName, string password, string server, int port, string security, string body, string to);
    }
}
