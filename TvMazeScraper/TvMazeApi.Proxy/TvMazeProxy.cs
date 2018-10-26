using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using TvMazeApi.Proxy.Dto;
using TvMazeApi.Proxy.Interfaces;
using TvMazeApi.Proxy.Utils;
using TvMazeApi.Proxy.Utils.Interfaces;
using TvMazeScraper.Models;

namespace TvMazeApi.Proxy
{
    public class TvMazeProxy : ITvMazeProxy
    {
        private readonly IApiClient _client;
        private readonly IQueryBuilder _queryBuilder;
        private readonly IMapper _mapper;

        public TvMazeProxy(IApiClient client, IQueryBuilder queryBuilder, IMapper mapper)
        {
            _client = client;
            _queryBuilder = queryBuilder;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Show>> GetShowsInfoByNameAsync(string showName)
        {
            var query = _queryBuilder.GetShowsByName(showName);
            var apiResult = (await _client.GetAsync<IEnumerable<ShowSearchResponse>>(query)).Select(c => c.Show).ToList();

            return await GetEnrichedShowsByIdRangeAsync(apiResult.Select(c => c.Id));
        }

        public async Task<Show> GetShowInfoWithCastByIdAsync(int id)
        {
            var query = _queryBuilder.GetShowById(id, EmbedParam.Cast);
            var apiResult = (await _client.GetAsync<ShowDto>(query));

            return _mapper.Map<ShowDto, Show>(apiResult);
        }

        public async Task<IEnumerable<Show>> GetShowsInfoByPageAsync(int page)
        {
            var query = _queryBuilder.GetShowsByPage(page);
            var apiResult = (await _client.GetAsync<IEnumerable<ShowDto>>(query, HttpStatusCode.NotFound))?.ToList();
            if (apiResult == null || !apiResult.Any()) return null;

            return await GetEnrichedShowsByIdRangeAsync(apiResult.Select(c => c.Id));
        }

        public async Task<IEnumerable<Show>> GetShowsUpdatesAsync(DateTime lastSyncDate)
        {
            var query = _queryBuilder.GetShowUpdates();
            var apiResult = (await _client.GetAsync<Dictionary<string, int?>>(query))?.ToList();

            var result = apiResult?.Select(c => new
            {
                Id = int.TryParse(c.Key, out int parsedId) ? parsedId : (int?)null,
                Date = TryParseToDate(c.Value?.ToString(), out DateTime parsedDate) ? (DateTime?)parsedDate : (DateTime?)null
            })
            .Where(c => c.Id.HasValue && c.Date.HasValue && c.Date.Value >= lastSyncDate)
            .Select(c => new
            {
                Id = c.Id.Value
            })
            .ToList();

            return result != null && result.Any() ? await GetEnrichedShowsByIdRangeAsync(result.Select(c => c.Id)) : null;
        }

        private bool TryParseToDate(string timestamp, out DateTime value)
        {
            try
            {
                if (long.TryParse(timestamp, out long convertedTimestamp))
                {
                    value = DateTimeOffset.FromUnixTimeSeconds(convertedTimestamp).DateTime;
                    return true;
                }
            }
            catch (Exception)
            {
                //empty handler
            }

            value = default(DateTime);
            return false;
        }

        private async Task<IEnumerable<Show>> GetEnrichedShowsByIdRangeAsync(IEnumerable<int> ids)
        {
            var result = new List<Show>();
            foreach (var id in ids)
            {
                var enrichedShow = await GetShowInfoWithCastByIdAsync(id);
                result.Add(enrichedShow);
            }
            return result;
        }
    }
}

