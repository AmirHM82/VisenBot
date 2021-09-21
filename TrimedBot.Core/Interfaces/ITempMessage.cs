using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Interfaces
{
    public interface ITempMessage
    {
        Task AddAsync(List<TempMessage> tempMessages);
        Task AddAsync(TempMessage tempMessages);
        Task<List<TempMessage>> GetAndDeleteAsync(long userId);
        Task<List<TempMessage>> GetAndDeleteAsync(long chatId, TempType type);
        Task<TempMessage> FindAsync(long userId, int messageId);
        void Delete(TempMessage tempMessage);
        Task Delete(long userId, int messageId);
        void Delete(List<TempMessage> tempMessages);
        Task SaveAsync();
    }
}
