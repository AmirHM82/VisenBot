using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Database.Models;

namespace TrimedBot.Core.Interfaces
{
    public interface ISettings
    {
        Task AddOrUpdateAsync(Settings settings);
        Task Add(Settings settings);
        Task SaveAsync();
        void Update(Settings settings);
        Task<Settings> GetSettings();
        Task<int> GetNumberOfSettings();
    }
}
