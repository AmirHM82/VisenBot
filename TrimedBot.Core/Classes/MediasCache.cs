using System.Collections.Generic;
using System.Threading.Tasks;
using TrimedBot.Core.Services;

namespace TrimedBot.Core.Classes
{
    public static class MediasCache
    {
        public static async Task CacheMedias(this CacheService cache, string key, IEnumerable<DAL.Entities.Media> medias)
        {
            await cache.SetAsync(key, medias);
            cache.keys.Add(key);
        }

        public static Task<List<DAL.Entities.Media>> GetCachedMedias(this CacheService cache, string key)
        {
            return cache.GetAsync<List<DAL.Entities.Media>>(key);
        }
    }
}
