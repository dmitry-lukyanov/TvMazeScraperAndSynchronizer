using Microsoft.Extensions.Configuration;
using TvMazeScraper.Api.Synchronizer.Utils.Interfaces;

namespace TvMazeScraper.Api.Synchronizer.Utils
{
    public class ApiSettingsProvider : IApiSettingsProvider
    {
        private readonly IConfiguration _configuration;

        private const string FullEnrichmentPeriodKey = "FullEnrichmentPeriod";
        private const string EnrichmentByUpdatingPeriodKey = "EnrichmentByUpdatingPeriod";
        private const string EnrichmentByUpdatingDelayKey = "EnrichmentByUpdatingDelay";
        private const string SynchronizationSection = "Synchronization";
        private const string FullEnrichmentEnabledKey = "FullEnrichmentEnabled";
        private const string EnrichmentByUpdatingEnabledKey = "EnrichmentByUpdatingEnabled";

        public ApiSettingsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int FullEnrichmentPeriod => _configuration.GetSection(SynchronizationSection).GetValue<int>(FullEnrichmentPeriodKey);
        public int EnrichmentByUpdatingPeriod => _configuration.GetSection(SynchronizationSection).GetValue<int>(EnrichmentByUpdatingPeriodKey);
        public int EnrichmentByUpdatingDelay => _configuration.GetSection(SynchronizationSection).GetValue<int>(EnrichmentByUpdatingDelayKey);
        public bool FullEnrichmentEnabled => _configuration.GetSection(SynchronizationSection).GetValue<bool>(FullEnrichmentEnabledKey);
        public bool EnrichmentByUpdatingEnabled => _configuration.GetSection(SynchronizationSection).GetValue<bool>(EnrichmentByUpdatingEnabledKey);
        
    }
}
