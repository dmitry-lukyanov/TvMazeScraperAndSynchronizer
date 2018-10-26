using System.Net;
using System.Threading.Tasks;

namespace TvMazeApi.Proxy.Utils.Interfaces
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string relatedUri, params HttpStatusCode[] allowedUnsuccessCodes);
    }
}
