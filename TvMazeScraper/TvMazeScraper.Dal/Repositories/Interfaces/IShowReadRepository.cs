using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TvMazeScraper.Models;

namespace TvMazeScraper.Dal.Repositories.Interfaces
{
    public interface IShowReadRepository
    {
        Task<IEnumerable<Show>> GetShowsAsync(Expression<Func<Show, bool>> expression, int page, int pageSize);
        Task<IEnumerable<Cast>> GetCastAsync(Expression<Func<Cast, bool>> expression, int page, int pageSize);
    }
}
