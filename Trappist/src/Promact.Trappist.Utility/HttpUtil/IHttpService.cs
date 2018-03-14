using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Trappist.Utility.HttpUtil
{
    public interface IHttpService
    {
        /// <summary>
        /// Makes a post request to the specified URL 
        /// </summary>
        /// <param name="url">Server URl</param>
        /// <param name="content">Content to be send</param>
        /// <returns>HttpResponseMessage</returns>
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content); 
    }
}
