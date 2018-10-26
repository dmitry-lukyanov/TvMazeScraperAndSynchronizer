using System.Collections.Generic;
using System.Threading.Tasks;

namespace TvMazeScraper.Synchronizer.Dal.Repositories.Interfaces
{
    public interface IProcessingStateRepository
    {
        Task<IEnumerable<int>> GetRequiredReProcessingPagesAsync();
        Task SuccessfulPageProcessedAsync(int pageNumber);
        Task SuccessfulPageReprocessedAsync(int pageNumber);
        Task PageProcessingFailedAsync(int pageNumber, string errorMessage);
        Task<int?> GetLastProcessedPageAsync();
    }
}
