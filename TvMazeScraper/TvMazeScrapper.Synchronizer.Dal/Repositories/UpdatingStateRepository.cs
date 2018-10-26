using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Synchronizer.Dal.Context;
using TvMazeScraper.Synchronizer.Dal.Dto;
using TvMazeScraper.Synchronizer.Dal.Repositories.Interfaces;

namespace TvMazeScraper.Synchronizer.Dal.Repositories
{
    public class UpdatingStateRepository: IUpdatingStateRepository
    {
        private readonly TvMazeScraperContext _context;

        public UpdatingStateRepository(TvMazeScraperContext context)
        {
            _context = context;
        }
        public async Task<DateTime?> GetLastUpdatedDateAsync()
        {
            return (await _context.SynchronizerStateDto.FirstOrDefaultAsync())?.LastUpdatedDate;
        }

        public async Task SetLastUpdatedDateAsync(DateTime updatedDate)
        {
            var previousVersion = await _context.SynchronizerStateDto.FirstOrDefaultAsync();
            if (previousVersion != null)
            {
                previousVersion.LastUpdatedDate = updatedDate;
            }
            else
            {
                var value = new SynchronizerStateDto();
                value.LastUpdatedDate = updatedDate;
                value.StateId = 1;
                await _context.SynchronizerStateDto.AddAsync(value);
            }
            await _context.SaveChangesAsync();
        }
    }
}
