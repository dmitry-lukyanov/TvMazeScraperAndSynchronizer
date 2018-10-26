using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TvMazeScraper.Synchronizer.Services.Interfaces;

namespace TvMazeScraper.Api.Synchronizer.Extensions
{
    public static class ProcessingStateServiceExtensions
    {
        public static async Task TrySetSuccessfulPageProcessedAsync(this IProcessingStateService processingStateService, int page, ILogger logger)
        {
            try
            {
                await processingStateService.SuccessfulPageProcessedAsync(page);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Updating for page processing status failed. Page:{page}, Status page:Processed");
            }
        }

        public static async Task TrySetSuccessfulPageReprocessedAsync(this IProcessingStateService processingStateService, int page, ILogger logger)
        {
            try
            {
                await processingStateService.SuccessfulPageReprocessedAsync(page);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Updating for page processing status failed. Page:{page}, Status page:Reprocessed");
            }
        }

        public static async Task TrySetPageProcessingFailedAsync(this IProcessingStateService processingStateService, int page, string errorMessage, ILogger logger)
        {
            try
            {
                await processingStateService.PageProcessingFailedAsync(page, errorMessage);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Updating for page processing status failed. Page:{page}, Status page:Failed");
            }
        }
    }
}
