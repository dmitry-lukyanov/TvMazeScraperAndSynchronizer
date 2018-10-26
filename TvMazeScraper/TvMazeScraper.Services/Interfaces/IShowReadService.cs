using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TvMazeScraper.Models;

namespace TvMazeScraper.Services.Interfaces
{
    public interface IShowReadService
    {
        Task<Show> GetShowAsync(int id);
        Task<IEnumerable<Show>> GetShowAsync(Expression<Func<Show, bool>> expression, int page, int pageSize);
        

        Task<IEnumerable<Cast>> GetCastAsync(int showId, int page, int pageSize);
        Task<IEnumerable<Cast>> GetCastAsync(int? id, string name, int page, int pageSize);
    }
}
