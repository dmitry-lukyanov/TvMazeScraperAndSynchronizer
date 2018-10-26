using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Models;
using TvMazeScraper.Synchronizer.Dal.Repositories.Interfaces;
using TvMazeScraper.Synchronizer.Services.Interfaces;

namespace TvMazeScraper.Synchronizer.Services
{
    public class ShowWriteService : IShowWriteService
    {
        private readonly IShowWriteRepository _shoWriteRepository;

        public ShowWriteService(IShowWriteRepository showWriteRepository)
        {
            _shoWriteRepository = showWriteRepository;
        }

        public async Task<int> AddOrUpdateAsync(Show show)
        {
            return await _shoWriteRepository.AddOrUpdateAsync(show);
        }


        public async Task AddOrUpdateRangeAsync(IEnumerable<Show> shows)
        {
            await _shoWriteRepository.AddOrUpdateRangeAsync(shows);
        }
    }
}
