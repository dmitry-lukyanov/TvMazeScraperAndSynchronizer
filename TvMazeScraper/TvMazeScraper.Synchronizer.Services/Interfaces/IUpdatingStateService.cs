using System;
using System.Threading.Tasks;

namespace TvMazeScraper.Synchronizer.Services.Interfaces
{
    public interface IUpdatingStateService
    {
        Task<DateTime?> GetLastUpdatedDateAsync();
        Task SetLastUpdatedDateAsync(DateTime updatedDate);
    }
}
