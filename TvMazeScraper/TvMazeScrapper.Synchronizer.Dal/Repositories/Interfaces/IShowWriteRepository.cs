using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Models;

namespace TvMazeScraper.Synchronizer.Dal.Repositories.Interfaces
{
    public interface IShowWriteRepository
    {
        Task<int> AddOrUpdateAsync(Show show);
        Task AddOrUpdateRangeAsync(IEnumerable<Show> shows);
    }
}
