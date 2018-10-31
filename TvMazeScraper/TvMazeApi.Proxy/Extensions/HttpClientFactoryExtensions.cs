using System;
using System.Net.Http;

namespace TvMazeApi.Proxy.Extensions
{
    public static class HttpClientFactoryExtensions
    {
        public static HttpClient CreateClient(this IHttpClientFactory factory, Uri uri)
        {
            var client = factory.CreateClient();
            client.BaseAddress = uri;
            return client;
        }
    }
}
