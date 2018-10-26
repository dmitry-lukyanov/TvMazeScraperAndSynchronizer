using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Dal.Context;
using TvMazeScraper.Dal.Repositories.Interfaces;
using TvMazeScraper.Models;

namespace TvMazeScraper.Dal.Repositories
{
    public class ShowReadRepository : IShowReadRepository
    {
        private readonly TvMazeScraperContext _context;

        public ShowReadRepository(TvMazeScraperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Show>> GetShowsAsync(Expression<Func<Show, bool>> expression, int page, int pageSize)
        {
            if (expression == null) expression = show => true;

            var result = await _context.Shows
                .Include(c => c.Cast)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<Show>()
                .Where(expression)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Cast>> GetCastAsync(Expression<Func<Cast, bool>> expression, int page, int pageSize)
        {
            if (expression == null) expression = show => true;

            var result = await _context.Cast
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<Cast>()
                .Where(expression)
                .ToListAsync();

            return result;
        }
    }
}
