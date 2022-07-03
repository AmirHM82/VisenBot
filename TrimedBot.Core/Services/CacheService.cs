using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Services
{
    public class CacheService
    {
        private IDistributedCache cache;

        public CacheService(IDistributedCache cache)
        {
            this.cache = cache;
            keys = new List<string>();
        }

        public async Task SetAsync(string key, object item)
        {
            if (item is null || key is null) return;

            string jsonedItem = JsonConvert.SerializeObject(item);
            await cache.SetStringAsync(key, jsonedItem);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (key is null) return default(T);

            var jsonedItem = await cache.GetStringAsync(key);
            if (jsonedItem == null) return default(T);
            var result = JsonConvert.DeserializeObject<T>(jsonedItem);
            return result;
        }

        public async Task RemoveAsync(string key)
        {
            if (key is null) return;

            await cache.RemoveAsync(key);
        }

        // Wtf am I doing
        public List<string> keys { get; set; }

        /*
        public void AddKey(string key) => keys.Add(key);

        public void RemoveKey(string key) => keys.Remove(key);

        public List<string> SearchInKeys(string key) => keys.Where(x => x.Contains(key)).ToList();

        public string FindKey(string key) => keys.FirstOrDefault(x => x == key);
        */

        // Don't remind about the rest of the code :/
        /*
        private Dictionary<string, List<Media>> objects = new Dictionary<string, List<Media>>();

        public bool Add(string key, List<Media> medias)
        {
            bool result = false;
            if (medias is null || key is null) return result;

            result = objects.TryAdd(key, medias);
            return result;
        }

        public bool Remove(string key)
        {
            bool result = false;
            if (key == null) return result;

            result = objects.Remove(key);
            return result;
        }

        public List<Media> Get(string key)
        {
            if (key == null) return null;

            objects.TryGetValue(key, out var medias);
            return medias;
        }
        */
    }
}
