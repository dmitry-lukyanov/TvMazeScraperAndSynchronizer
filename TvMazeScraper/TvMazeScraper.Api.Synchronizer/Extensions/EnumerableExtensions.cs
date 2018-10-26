using System.Collections.Generic;
using System.Linq;

namespace TvMazeScraper.Api.Synchronizer.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T?> ConvertToNullableInts<T>(this IEnumerable<T> collection) where T : struct
        {
            if (collection == null)
            {
                return Enumerable.Empty<T?>();
            }
            return new List<T?>(collection.Select(c => (T?)c));
        }
    }
}
