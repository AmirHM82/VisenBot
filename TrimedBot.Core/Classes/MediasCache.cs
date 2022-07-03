using System.Collections.Generic;
using System.Linq;
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

        public static Task<IEnumerable<DAL.Entities.Media>> GetCachedMedias(this CacheService cache, string key)
        {
            return cache.GetAsync<IEnumerable<DAL.Entities.Media>>(key);
        }

        public static async Task EditCachedMedias(this CacheService cache, string[] searchTexts, IEnumerable<DAL.Entities.Media> medias)
        { // Test this method
            List<string> keys = new();
            foreach (var text in searchTexts)
            {
                keys.AddRange(cache.keys.Where(keys => keys.Contains(text)));
            }
            if (keys.Count == 0) return;

            foreach (var key in keys)
            {
                var cachedMedias = (await cache.GetAsync<IEnumerable<DAL.Entities.Media>>(key)).ToList();

                List<DAL.Entities.Media> differentMedias = new();
                foreach (var media in medias)
                {
                    differentMedias.AddRange(cachedMedias.Where(x => x.Id != media.Id));
                }

                cachedMedias.Clear();
                cachedMedias.AddRange(medias);
                if (differentMedias is not null || differentMedias.Count is not 0)
                    cachedMedias.AddRange(differentMedias);

                await cache.RemoveAsync(key);
                await cache.CacheMedias(key, cachedMedias);
            }
        }
    }
}
