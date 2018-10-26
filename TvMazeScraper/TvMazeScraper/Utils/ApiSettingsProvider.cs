using Microsoft.Extensions.Configuration;
using TvMazeScraper.Utils.Interfaces;

namespace TvMazeScraper.Utils
{
    public class ApiSettingsProvider : IApiSettingsProvider
    {
        private readonly IConfiguration _configuration;

        private const string PageSizeKey = "DefaultPageSize";

        public ApiSettingsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int DefaultPageSize => _configuration.GetValue<int>(PageSizeKey);
    }
}
