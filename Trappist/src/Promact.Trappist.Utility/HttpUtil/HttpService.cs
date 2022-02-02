using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Trappist.Utility.HttpUtil
{
    [ExcludeFromCodeCoverage]
    public class HttpService : IHttpService
    {
        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            using HttpClient client = new HttpClient();
            return await client.PostAsync(url, content);
        }
    }
}
