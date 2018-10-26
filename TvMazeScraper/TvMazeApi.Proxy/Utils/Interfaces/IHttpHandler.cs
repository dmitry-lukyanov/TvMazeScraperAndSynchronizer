using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TvMazeApi.Proxy.Utils.Interfaces
{
    public interface IHttpHandler
    {
        Task<HttpResponseMessage> GetAsync(Uri url, HttpCompletionOption httpCompletionOption);
    }
}
