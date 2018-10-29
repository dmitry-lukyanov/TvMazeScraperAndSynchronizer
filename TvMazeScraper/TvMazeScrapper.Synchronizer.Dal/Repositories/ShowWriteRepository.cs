using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        private const int UpdatedStatus = 0;
        private const int CreatedStatus = 1;

        public ShowWriteRepository(TvMazeScraperContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            _mapper.Map(show, showDto);

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
            show.Cast?.ToList().ForEach(cast =>
            {
                if (showDto.Cast == null) showDto.Cast = new List<CastDto>();

                var castDto = showDto.Cast.FirstOrDefault(d => d.Id == cast.Id);
                if (castDto == null)
                {
                    castDto = new CastDto();
                    showDto.Cast.Add(castDto);
                }
                _mapper.Map(cast, castDto);
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
