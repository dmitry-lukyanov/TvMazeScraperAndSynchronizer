using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Synchronizer.Dal.Context;
using TvMazeScraper.Synchronizer.Dal.Dto;
using TvMazeScraper.Synchronizer.Dal.Repositories.Interfaces;

namespace TvMazeScraper.Synchronizer.Dal.Repositories
{
    public class ProcessingStateRepository : IProcessingStateRepository
    {
        private readonly TvMazeScraperContext _context;
        private enum ProcessingState
        {
            SuccessfullReprocessed = 1,
            SuccessfullProcessed = 0,
            Failed = -1
        }

        public ProcessingStateRepository(TvMazeScraperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<int>> GetRequiredReProcessingPagesAsync()
        {
            return await _context.PagesProcessingStatuses.Where(c => c.Status == (int)ProcessingState.Failed).Select(c => c.Id).ToListAsync();
        }

        public async Task<int?> GetLastProcessedPageAsync()
        {
            return (await _context.PagesProcessingStatuses.Where(c => c.Status != (int)ProcessingState.Failed)
                    .OrderByDescending(c => c.Id)
                    .FirstOrDefaultAsync()
                    )?.Id;
        }

        public async Task SuccessfulPageProcessedAsync(int pageNumber)
        {
            await SetPageProcessingStateAsync(pageNumber, ProcessingState.SuccessfullProcessed, null);
        }

        public async Task SuccessfulPageReprocessedAsync(int pageNumber)
        {
            await SetPageProcessingStateAsync(pageNumber, ProcessingState.SuccessfullReprocessed, null);
        }

        public async Task PageProcessingFailedAsync(int pageNumber, string errorMessage)
        {
            await SetPageProcessingStateAsync(pageNumber, ProcessingState.Failed, errorMessage);
        }

        private async Task SetPageProcessingStateAsync(int pageNumber, ProcessingState state, string errorMessage)
        {
            var previousVersion = await _context.PagesProcessingStatuses.FirstOrDefaultAsync(c => c.Id == pageNumber);
            if (previousVersion != null)
            {
                switch (state)
                {
                    case ProcessingState.SuccessfullProcessed:
                        throw new ArgumentException($"Atempt to save the page {pageNumber} which was already saved before");
                    case ProcessingState.SuccessfullReprocessed:
                    case ProcessingState.Failed:
                        previousVersion.Status = (int)state;
                        previousVersion.AttemptToProcess++;
                        previousVersion.LastError = errorMessage;
                        break;
                }
            }
            else
            {
                switch (state)
                {
                    case ProcessingState.SuccessfullReprocessed:
                        throw new ArgumentException($"Attempt to set success reprocessed state failed. There is no the page {pageNumber} in the unprocessed list");
                    case ProcessingState.SuccessfullProcessed:
                    case ProcessingState.Failed:
                        {
                            var pageProcessingStatus = new PageProcessingStatus();
                            pageProcessingStatus.AttemptToProcess = 1;
                            pageProcessingStatus.Id= pageNumber;
                            pageProcessingStatus.Status = (int)state;
                            pageProcessingStatus.LastError = errorMessage;
                            _context.PagesProcessingStatuses.Add(pageProcessingStatus);
                        }
                        break;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
