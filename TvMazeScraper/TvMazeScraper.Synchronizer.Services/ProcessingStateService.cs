using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Synchronizer.Dal.Repositories.Interfaces;
using TvMazeScraper.Synchronizer.Services.Interfaces;

namespace TvMazeScraper.Synchronizer.Services
{
    public class ProcessingStateService : IProcessingStateService
    {
        private readonly IProcessingStateRepository _processingStateRepository;

        public ProcessingStateService(IProcessingStateRepository processingStateRepository)
        {
            _processingStateRepository = processingStateRepository;
        }

        public async Task PageProcessingFailedAsync(int pageNumber, string lastError)
        {
            await _processingStateRepository.PageProcessingFailedAsync(pageNumber, lastError);
        }

        public async Task SuccessfulPageReprocessedAsync(int pageNumber)
        {
            await _processingStateRepository.SuccessfulPageReprocessedAsync(pageNumber);
        }
        public async Task SuccessfulPageProcessedAsync(int pageNumber)
        {
            await _processingStateRepository.SuccessfulPageProcessedAsync(pageNumber);
        }

        public async Task<IEnumerable<int>> GetRequiredReProcessingPagesAsync()
        {
            return await _processingStateRepository.GetRequiredReProcessingPagesAsync();
        }

        public async Task<int?> GetLastProcessedPageAsync()
        {
            return await _processingStateRepository.GetLastProcessedPageAsync();
        }
    }
}
