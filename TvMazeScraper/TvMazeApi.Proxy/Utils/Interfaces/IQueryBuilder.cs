namespace TvMazeApi.Proxy.Utils.Interfaces
{
    public interface IQueryBuilder
    {
        string GetShowsByName(string query);
        string GetSingleShowByName(string query);
        string GetSingleShowByName(string query, EmbedParam? embed);
        string GetShowById(int id);
        string GetShowById(int id, EmbedParam? embed);
        string GetShowsByPage(int page);
        string GetShowUpdates();
    }
}
