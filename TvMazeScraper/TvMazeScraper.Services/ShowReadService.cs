using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TvMazeScraper.Dal.Repositories.Interfaces;
using TvMazeScraper.Models;
using TvMazeScraper.Services.Interfaces;

namespace TvMazeScraper.Services
{
    public class ShowReadService : IShowReadService
    {
        private readonly IShowReadRepository _showReadRepository;

        public ShowReadService(IShowReadRepository showReadRepository)
        {
            _showReadRepository = showReadRepository;
        }

        public async Task<Show> GetShowAsync(int id)
        {
            return (await _showReadRepository.GetShowsAsync(show => show.Id == id, 1, 1)).FirstOrDefault();
        }

        public async Task<IEnumerable<Show>> GetShowAsync(Expression<Func<Show, bool>> expression, int page, int pageSize)
        {
            return (await _showReadRepository.GetShowsAsync(expression, page, pageSize));
        }

        public async Task<IEnumerable<Cast>> GetCastAsync(int showId, int page, int pageSize)
        {
            return await _showReadRepository.GetCastAsync(cast => cast.ShowId == showId, page, pageSize);
        }

        public async Task<IEnumerable<Cast>> GetCastAsync(int? id, string name, int page, int pageSize)
        {
            return await _showReadRepository.GetCastAsync(cast => cast.Id == (id ?? cast.Id) && cast.Name == (!string.IsNullOrWhiteSpace(name) ? name : cast.Name), page, pageSize);
        }
    }
}
