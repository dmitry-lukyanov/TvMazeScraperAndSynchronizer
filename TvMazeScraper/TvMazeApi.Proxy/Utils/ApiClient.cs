using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TvMazeApi.Proxy.Utils.Interfaces;

namespace TvMazeApi.Proxy.Utils
{
    public class ApiClient : IApiClient
    {
        private Uri BaseEndpoint { get; }
        private readonly IHttpHandler _httpHandler;
        private readonly IThreadUtil _threadUtil;
        private readonly IProxySettingsProvider _proxySettingsProvider;

        public ApiClient(IProxySettingsProvider settingsProvider, IHttpHandler httpHandler, IThreadUtil threadUtil, IProxySettingsProvider proxySettingsProvider)
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
            _httpHandler = httpHandler;
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
                response = await _httpHandler.GetAsync(fullUrl, HttpCompletionOption.ResponseHeadersRead);
                isTryOnceAgain = response.StatusCode == HttpStatusCode.TooManyRequests;
                if (isTryOnceAgain) await _threadUtil.DelayAsync(tooManayRequestsDelay);
                attempt++;
            } while (isTryOnceAgain && attempt < maxAttemptsForTooManayRequests);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return ParseTo<T>(data);
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

        private static T ParseTo<T>(string originalMessage)
        {
            var token = JToken.Parse(originalMessage);
            return token.ToObject<T>(JsonSerializer.Create(SerializationSettings));
        }

        private static JsonSerializerSettings SerializationSettings => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
        };
    }
}
