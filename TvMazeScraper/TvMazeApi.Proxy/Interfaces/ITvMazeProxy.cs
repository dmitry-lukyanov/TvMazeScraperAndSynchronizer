using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Models;

namespace TvMazeApi.Proxy.Interfaces
{
    public interface ITvMazeProxy
    {
        Task<IEnumerable<Show>> GetShowsInfoByNameAsync(string showName);
        Task<Show> GetShowInfoWithCastByIdAsync(int id);
        Task<IEnumerable<Show>> GetShowsInfoByPageAsync(int page);
        Task<IEnumerable<Show>> GetShowsUpdatesAsync(DateTime lastSyncDate);
    }
}
