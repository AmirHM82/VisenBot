using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Context;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Services
{
    public class SettingsServices : ISettings
    {
        protected DB _db;

        public SettingsServices(DB db)
        {
            _db = db;
        }

        public Task Add(Settings settings)
        {
            return Task.Run(async () => await _db.Settings.AddAsync(settings));
        }

        public Task AddOrUpdateAsync(Settings settings)
        {
            return Task.Run(async () =>
            {
                var num = await GetNumberOfSettings();
                if (num == 1) Update(settings);
                else if (num == 0) await _db.Settings.AddAsync(settings);
                else throw new Exception("Your settings table is more than 1, and it causes problem");
            });
        }

        public Task<int> GetNumberOfSettings()
        {
            return Task.Run(() =>
            {
                var num = _db.Settings.Count();
                return num;
            });
        }

        public Task<Settings> GetSettings()
        {
            return Task.Run(() =>
            {
                var settings = _db.Settings.OrderBy(x=>x.Id).LastOrDefault();
                return settings;
            });
        }

        public Task SaveAsync()
        {
            return Task.Run(async () =>
            {
                await _db.SaveChangesAsync();
            });
        }

        public void Update(Settings settings)
        {
            _db.Settings.Update(settings);
        }
    }
}
