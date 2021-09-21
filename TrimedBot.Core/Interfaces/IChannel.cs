using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Interfaces
{
    public interface IChannel : IDisposable
    {
        Task<List<Channel>> GetChannelsAsync();
        Task AddAsync(Channel channel);
        Task<Channel> FindAsync(long chatId);
        Task<Channel> FindAsync(int id);
        Task<Channel> FindOrAddAsync(Channel channel);
        Task<bool> Exist(long chatId);
        void Delete(Channel channel);
        void Update(Channel channel);
        Task SaveAsync();
        Task AddPostAsync(ChannelPost channelPost);
        Task AddPostAsync(List<ChannelPost> channelPosts);
    }
}
