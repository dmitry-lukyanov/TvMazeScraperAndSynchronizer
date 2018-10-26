using System;

namespace TvMazeScraper.Api.Synchronizer.Utils.Interfaces
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
    }
}
