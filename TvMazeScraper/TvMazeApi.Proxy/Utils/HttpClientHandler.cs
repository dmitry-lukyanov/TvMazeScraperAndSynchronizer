using System;
using System.Net.Http;
using System.Threading.Tasks;
using TvMazeApi.Proxy.Utils.Interfaces;

namespace TvMazeApi.Proxy.Utils
{
    public class HttpClientHandler : IHttpHandler
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<HttpResponseMessage> GetAsync(Uri url, HttpCompletionOption httpCompletionOption)
        {
            return await _client.GetAsync(url, httpCompletionOption);
        }
    }
}
