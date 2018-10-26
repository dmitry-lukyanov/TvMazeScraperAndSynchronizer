using TvMazeApi.Proxy.Utils.Interfaces;

namespace TvMazeApi.Proxy.Utils
{
    public class QueryBuilder : IQueryBuilder
    {
        public string GetShowsByName(string query) => $"search/shows?q={query}";

        public string GetSingleShowByName(string query) => GetSingleShowByName(query, null);

        public string GetSingleShowByName(string query, EmbedParam? embed)
        {
            var result = $"singlesearch/shows?q={query}";
            
            return AddEmbedIfRequired(result, embed);
        }

        public string GetShowById(int id) => GetShowById(id, null);

        public string GetShowById(int id, EmbedParam? embed)
        {
            var result = $"shows/{id}";
            return AddEmbedIfRequired(result, embed);
        }

        public string GetShowsByPage(int page) => $"shows?page={page}";

        public string GetShowUpdates() => $"updates/shows";

        private string AddEmbedIfRequired(string query, EmbedParam? embed)
        {
            if (embed.HasValue)
            {
                var httpParamPrefix = !query.Contains("?") ? "?" : "&";
                query += $"{httpParamPrefix}embed={embed.Value.ToString().ToLower()}";
            }
            return query;
        }
    }
}
