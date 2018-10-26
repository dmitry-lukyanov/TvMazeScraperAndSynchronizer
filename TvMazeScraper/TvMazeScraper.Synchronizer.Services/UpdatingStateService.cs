using System;
using System.Threading.Tasks;
using TvMazeScraper.Synchronizer.Dal.Repositories.Interfaces;
using TvMazeScraper.Synchronizer.Services.Interfaces;

namespace TvMazeScraper.Synchronizer.Services
{
    public class UpdatingStateService : IUpdatingStateService
    {
        private readonly IUpdatingStateRepository _updatingStateRepository;

        public UpdatingStateService(IUpdatingStateRepository updatingStateRepository)
        {
            _updatingStateRepository = updatingStateRepository;
        }

        public async Task<DateTime?> GetLastUpdatedDateAsync()
        {
            return await _updatingStateRepository.GetLastUpdatedDateAsync();
        }

        public async Task SetLastUpdatedDateAsync(DateTime updatedDate)
        {
            await _updatingStateRepository.SetLastUpdatedDateAsync(updatedDate);
        }
    }
}
