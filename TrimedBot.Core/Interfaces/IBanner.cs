using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Interfaces
{
    public interface IBanner
    {
        Task AddAsync(Banner banner);
        void Delete(Banner banner);
        Task<Banner> FindAsync(Guid id);
        Task<List<Banner>> GetAllAsync();
        Task SaveAsync();
    }
}
