using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Trappist.Utility.HttpUtil
{
    public class HttpService : IHttpService
    {
        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            HttpClient client = new HttpClient();
            return await client.PostAsync(url, content);
        }
    }
}
