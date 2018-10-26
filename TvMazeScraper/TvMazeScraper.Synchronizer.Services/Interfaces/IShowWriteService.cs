using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Models;

namespace TvMazeScraper.Synchronizer.Services.Interfaces
{
    public interface IShowWriteService
    {
        Task<int> AddOrUpdateAsync(Show show);
        Task AddOrUpdateRangeAsync(IEnumerable<Show> shows);
    }
}
