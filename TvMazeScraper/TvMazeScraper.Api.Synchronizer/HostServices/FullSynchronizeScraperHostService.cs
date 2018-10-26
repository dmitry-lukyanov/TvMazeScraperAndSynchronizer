using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TvMazeApi.Proxy.Interfaces;
using TvMazeScraper.Api.Synchronizer.Extensions;
using TvMazeScraper.Api.Synchronizer.HostServices.Entities;
using TvMazeScraper.Api.Synchronizer.Utils.Interfaces;
using TvMazeScraper.Synchronizer.Services.Interfaces;

namespace TvMazeScraper.Api.Synchronizer.HostServices
{
    public class FullSynchronizeScraperHostService : TimerHostService
    {
        private readonly IShowWriteService _showWriteService;
        private readonly ITvMazeProxy _tvMazeProxy;
        private readonly IProcessingStateService _processingStateService;
        private readonly IUpdatingStateService _updatingStateService;
        private readonly IApiSettingsProvider _apiSettingsProvider;
        private readonly ITimeProvider _timeProvider;

        public override TimeSpan Period => TimeSpan.FromMinutes(_apiSettingsProvider.FullEnrichmentPeriod);

        public override string HostName => nameof(FullSynchronizeScraperHostService);

        public override bool CanBeCalled => _apiSettingsProvider.FullEnrichmentEnabled;

        public FullSynchronizeScraperHostService(
            IShowWriteService showWriteService,
            ITvMazeProxy tvMazeProxy,
            ILogger<FullSynchronizeScraperHostService> logger,
            IProcessingStateService processingStateService,
            IApiSettingsProvider apiSettingsProvider,
            ITimeProvider timeProvider,
            IUpdatingStateService updatingStateService) : base(logger)
        {
            _showWriteService = showWriteService;
            _tvMazeProxy = tvMazeProxy;
            _processingStateService = processingStateService;
            _updatingStateService = updatingStateService;
            _apiSettingsProvider = apiSettingsProvider;
            _timeProvider = timeProvider;
        }

        public override async Task OnTimerCalledAsync(CancellationToken cancellationToken)
        {
            var lastPage = await _processingStateService.GetLastProcessedPageAsync();
            var needsToBeReprocessed = await _processingStateService.GetRequiredReProcessingPagesAsync();
            int startPage = lastPage + 1 ?? 0;

            foreach (var currentPageInfo in GeneratePageInfoBy(startPage, needsToBeReprocessed))
            {
                try
                {
                    Logger.LogInformation($"Scraper page {0} processing is started");

                    var itemResult = await _tvMazeProxy.GetShowsInfoByPageAsync(currentPageInfo.Page);
                    if (itemResult != null)
                    {
                        await _showWriteService.AddOrUpdateRangeAsync(itemResult);
                        if (currentPageInfo.IsReprocessing)
                        {
                            await _processingStateService.TrySetSuccessfulPageReprocessedAsync(currentPageInfo.Page, Logger);
                        }
                        else
                        {
                            await _processingStateService.TrySetSuccessfulPageProcessedAsync(currentPageInfo.Page, Logger);
                        }
                    }
                    else
                    {
                        await _updatingStateService.SetLastUpdatedDateAsync(_timeProvider.Now);
                        Logger.LogInformation($"Scraper page {0} processing is finished. There was no page to process");
                        break;
                    }
                    Logger.LogInformation($"Scraper page {0} processing is finished");
                }
                catch (Exception ex)
                {
                    await _processingStateService.TrySetPageProcessingFailedAsync(currentPageInfo.Page, ex.GetBaseException().Message, Logger);
                    Logger.LogCritical(ex, $"Full synchronize scraper host failed. Page number : {currentPageInfo.Page}");
                }
            }
        }

        public IEnumerable<PageProcessingInfo> GeneratePageInfoBy(int startPage, IEnumerable<int> needsToBeReprocessed)
        {
            List<int?> reprocessedPages = new List<int?>(needsToBeReprocessed.ConvertToNullableInts());
            int pageCollectionIndex = 0;
            bool normalIncrementMode = false;

            do
            {
                var reprocessedPage = reprocessedPages?.ElementAtOrDefault(pageCollectionIndex);
                if (normalIncrementMode || (reprocessedPage == null || startPage < reprocessedPage))
                {
                    normalIncrementMode = true;
                    yield return new PageProcessingInfo
                    {
                        Page = startPage,
                        IsReprocessing = reprocessedPages.Contains(startPage)
                    };
                    startPage++;
                }
                else
                {
                    pageCollectionIndex++;
                    yield return new PageProcessingInfo()
                    {
                        Page = reprocessedPage.Value,
                        IsReprocessing = true
                    };
                }
            } while (true);
        }
    }
}
