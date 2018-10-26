using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TvMazeApi.Proxy.Interfaces;
using TvMazeScraper.Api.Synchronizer.Utils.Interfaces;
using TvMazeScraper.Synchronizer.Services.Interfaces;

namespace TvMazeScraper.Api.Synchronizer.HostServices
{
    public class LastUpdatesSynchronizeScraperHostService : TimerHostService
    {
        private readonly IShowWriteService _showWriteService;
        private readonly ITvMazeProxy _tvMazeProxy;
        private readonly IUpdatingStateService _updatingStateService;
        private readonly IApiSettingsProvider _apiSettingsProvider;
        private readonly ITimeProvider _timeProvider;

        public override string HostName => nameof(LastUpdatesSynchronizeScraperHostService);
        public override TimeSpan Period => TimeSpan.FromMinutes(_apiSettingsProvider.EnrichmentByUpdatingPeriod);

        public override bool CanBeCalled => _apiSettingsProvider.EnrichmentByUpdatingEnabled;

        public LastUpdatesSynchronizeScraperHostService(
            IShowWriteService showWriteService, 
            ITvMazeProxy tvMazeProxy, 
            IUpdatingStateService updatingStateService,
            IApiSettingsProvider apiSettingsProvider,
            ITimeProvider timeProvider,
            ILogger<LastUpdatesSynchronizeScraperHostService> logger) : base(logger)
        {
            _showWriteService = showWriteService;
            _tvMazeProxy = tvMazeProxy;
            _updatingStateService = updatingStateService;
            _apiSettingsProvider = apiSettingsProvider;
            _timeProvider = timeProvider;
        }

        public override async Task OnTimerCalledAsync(CancellationToken cancellationToken)
        {
            var lastUpdatedDate = await _updatingStateService.GetLastUpdatedDateAsync();
            if (lastUpdatedDate.HasValue)
            {
                var changedShows = await _tvMazeProxy.GetShowsUpdatesAsync(lastUpdatedDate.Value);
                if (changedShows != null)
                {
                    await _showWriteService.AddOrUpdateRangeAsync(changedShows);
                    await _updatingStateService.SetLastUpdatedDateAsync(_timeProvider.Now);
                }
            }
        }

        public override TimeSpan Duetime => TimeSpan.FromMinutes(_apiSettingsProvider.EnrichmentByUpdatingDelay);
    }
}
