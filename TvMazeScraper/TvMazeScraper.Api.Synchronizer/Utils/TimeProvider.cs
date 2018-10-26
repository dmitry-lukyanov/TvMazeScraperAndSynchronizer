using System;
using TvMazeScraper.Api.Synchronizer.Utils.Interfaces;

namespace TvMazeScraper.Api.Synchronizer.Utils
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
