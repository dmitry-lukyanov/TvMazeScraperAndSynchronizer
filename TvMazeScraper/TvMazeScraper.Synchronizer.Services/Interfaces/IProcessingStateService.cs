using System.Collections.Generic;
using System.Threading.Tasks;

namespace TvMazeScraper.Synchronizer.Services.Interfaces
{
    public interface IProcessingStateService
    {
        Task PageProcessingFailedAsync(int pageNumber, string lastError);
        Task SuccessfulPageReprocessedAsync(int pageNumber);
        Task SuccessfulPageProcessedAsync(int pageNumber);

        Task<IEnumerable<int>> GetRequiredReProcessingPagesAsync();
        Task<int?> GetLastProcessedPageAsync();
    }
}
