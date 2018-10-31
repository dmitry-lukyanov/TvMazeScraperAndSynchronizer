using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TvMazeApi.Proxy.Extensions;
using TvMazeApi.Proxy.Utils.Interfaces;

namespace TvMazeApi.Proxy.Utils
{
    public class ApiClient : IApiClient
    {
        private Uri BaseEndpoint { get; }
        private readonly IThreadUtil _threadUtil;
        private readonly IProxySettingsProvider _proxySettingsProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiClient(IProxySettingsProvider settingsProvider, IHttpClientFactory httpClientFactory, IThreadUtil threadUtil, IProxySettingsProvider proxySettingsProvider)
        {
            if (settingsProvider == null)
            {
                throw new ArgumentNullException(nameof(settingsProvider));
            }

            if (string.IsNullOrWhiteSpace(settingsProvider.ApiUrl))
            {
                throw new ArgumentException("api settings");
            }

            BaseEndpoint = new Uri(settingsProvider.ApiUrl);
            _httpClientFactory = httpClientFactory;
            _threadUtil = threadUtil;
            _proxySettingsProvider = proxySettingsProvider;
        }

        public async Task<T> GetAsync<T>(string relatedUri, params HttpStatusCode[] allowedUnsuccessCodes)
        {
            int tooManayRequestsDelay = _proxySettingsProvider.DelayForTooManyRequestHttpError;
            int maxAttemptsForTooManayRequests = _proxySettingsProvider.MaxAttemptNumberForTooManyRequestsHttpError;

            var fullUrl = new Uri(new Uri(BaseEndpoint.AbsoluteUri), relatedUri);

            HttpResponseMessage response = null;

            bool isTryOnceAgain = false;
            int attempt = 0;
            do
            {
                response = await _httpClientFactory.CreateClient(BaseEndpoint).GetAsync(fullUrl, HttpCompletionOption.ResponseHeadersRead);
                isTryOnceAgain = response.StatusCode == HttpStatusCode.TooManyRequests;
                if (isTryOnceAgain) await _threadUtil.DelayAsync(tooManayRequestsDelay);
                attempt++;
            } while (isTryOnceAgain && attempt < maxAttemptsForTooManayRequests);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return data.ParseTo<T>();
            }
            else if (allowedUnsuccessCodes != null && allowedUnsuccessCodes.Contains(response.StatusCode))
            {
                return default(T);
            }
            else
            {
                response.EnsureSuccessStatusCode();
                return default(T);
            }
        }
    }
}
