using System;
using System.Threading.Tasks;

namespace TvMazeScraper.Synchronizer.Dal.Repositories.Interfaces
{
    public interface IUpdatingStateRepository
    {
        Task<DateTime?> GetLastUpdatedDateAsync();

        Task SetLastUpdatedDateAsync(DateTime updatedDate);
    }
}
