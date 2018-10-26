namespace TvMazeScraper.Api.Synchronizer.Utils.Interfaces
{
    public interface IApiSettingsProvider
    {
        int FullEnrichmentPeriod { get; }
        int EnrichmentByUpdatingPeriod { get; }
        int EnrichmentByUpdatingDelay { get; }
        bool FullEnrichmentEnabled { get; }
        bool EnrichmentByUpdatingEnabled { get; }
    }
}
