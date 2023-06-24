using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Interfaces
{
    public interface IChannelPost
    {
        ValueTask<EntityEntry<ChannelPost>> AddAsync(ChannelPost channelPost);
        Task AddAsync(List<ChannelPost> channelPosts);
        void Delete(ChannelPost channelPost);
        Task Delete(int messageId);
        Task Delete(long chatId, int messageId);
        Task<List<ChannelPost>> GetAndDelete(Guid mediaId);
        ValueTask<ChannelPost> FindAsync(int messageId);
        Task<ChannelPost> FindAsync(long chatdId, int messageId);
        Task<List<ChannelPost>> GetAndDelete(long chatId, PostType type);
        Task SaveAsync();
        void Update(ChannelPost channelPost);
    }
}
