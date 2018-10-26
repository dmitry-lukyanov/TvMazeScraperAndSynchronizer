using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Models;
using TvMazeScraper.Synchronizer.Dal.Context;
using TvMazeScraper.Synchronizer.Dal.Dto;
using TvMazeScraper.Synchronizer.Dal.Repositories.Interfaces;

namespace TvMazeScraper.Synchronizer.Dal.Repositories
{
    public class ShowWriteRepository : IShowWriteRepository
    {
        private readonly TvMazeScraperContext _context;

        private const int UpdatedStatus = 0;
        private const int CreatedStatus = 1;

        public ShowWriteRepository(TvMazeScraperContext context)
        {
            _context = context;
        }

        private async Task<int> AddOrUpdateWithoutSaveAsync(Show show)
        {
            int result = UpdatedStatus;
            var showDto = await _context.Shows.Include(c => c.Cast).FirstOrDefaultAsync(o => o.Id == show.Id);
            if (showDto == null)
            {
                result = CreatedStatus;
                showDto = new ShowDto();
                await _context.Shows.AddAsync(showDto);
            }

            showDto.Id = show.Id;
            showDto.Name = show.Name;

            var toDelete = new List<CastDto>();
            //remove deleted cast
            foreach (var deleted in showDto.Cast ?? Enumerable.Empty<CastDto>())
            {
                if (!show.Cast?.Any(c => c.Id == deleted.Id) ?? true)
                {
                    toDelete.Add(deleted);
                }
            }
            if (toDelete.Any())
            {
                _context.Cast.RemoveRange(toDelete);
            }

            //update or add details
            show.Cast?.ToList().ForEach(castDto =>
            {
                if (showDto.Cast == null) showDto.Cast = new List<CastDto>();

                var cast = showDto.Cast.FirstOrDefault(d => d.Id == castDto.Id);
                if (cast == null)
                {
                    cast = new CastDto();
                    showDto.Cast.Add(cast);
                }
                cast.Id = castDto.Id;
                cast.Name = castDto.Name;
                cast.Birthday = castDto.Birthday;
                cast.Name = castDto.Name;
                cast.ShowId = castDto.ShowId;
            });

            return result;
        }

        public async Task<int> AddOrUpdateAsync(Show show)
        {
            var result = await AddOrUpdateWithoutSaveAsync(show);

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task AddOrUpdateRangeAsync(IEnumerable<Show> shows)
        {
            foreach (var show in shows)
            {
                await AddOrUpdateWithoutSaveAsync(show);
            }

            await _context.SaveChangesAsync();
        }
    }
}
