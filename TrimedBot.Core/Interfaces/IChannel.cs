using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Interfaces
{
    public interface IChannel : IDisposable
    {
        /// <summary>
        /// Gets channels that was registered in bot
        /// </summary>
        /// <param name="isForAdmins">False for not getting the admin channels, true for getting them</param>
        /// <returns>A list of channels</returns>
        Task<List<Channel>> GetOtherChannelsAsync();
        Task<List<Channel>> GetAdminChannelsAsync();
        Task<List<Channel>> Channels();
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
        Task<Channel> ChangeType(int id, ChannelType type);
    }
}
