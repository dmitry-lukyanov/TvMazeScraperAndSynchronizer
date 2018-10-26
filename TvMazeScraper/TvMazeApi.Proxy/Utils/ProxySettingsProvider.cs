using Microsoft.Extensions.Configuration;
using TvMazeApi.Proxy.Utils.Interfaces;

namespace TvMazeApi.Proxy.Utils
{
    public class ProxySettingsProvider : IProxySettingsProvider
    {
        private const string ApiProxyUrlKey = "ApiProxyUrl";
        private const string ProxySection = "Proxy";
        private const string DelayForTooManyRequestHttpErrorKey = "DelayForTooManyRequestHttpError";

        private const string MaxAttemptNumberForTooManyRequestsHttpErrorKey = "MaxAttemptNumberForTooManyRequestsHttpError";

        private const int DelayForTooManyRequestHttpErrorDefault = 1000;
        private const int MaxAttemptNumberForTooManyRequestsHttpErrorDefault = 30;

        private readonly IConfiguration _configuration;

        public ProxySettingsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ApiUrl => 
            _configuration
            .GetSection(ProxySection)
            .GetValue<string>(ApiProxyUrlKey);

        public int MaxAttemptNumberForTooManyRequestsHttpError => 
            _configuration
            .GetSection(ProxySection)
            .GetValue<int?>(MaxAttemptNumberForTooManyRequestsHttpErrorKey) 
            ?? MaxAttemptNumberForTooManyRequestsHttpErrorDefault;

        /// <summary>
        /// Milliseconds
        /// </summary>
        public int DelayForTooManyRequestHttpError => 
            _configuration
            .GetSection(ProxySection)
            .GetValue<int?>(DelayForTooManyRequestHttpErrorKey) 
            ?? DelayForTooManyRequestHttpErrorDefault;
    }
}
