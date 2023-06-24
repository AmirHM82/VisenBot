using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Context;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Enums;

namespace TrimedBot.Core.Services
{
    public class ChannelService : IChannel
    {
        private DB db;

        public ChannelService(DB db)
        {
            this.db = db;
        }

        public async Task AddAsync(Channel channel)
        {
            await db.Channels.AddAsync(channel);
        }

        public async Task AddPostAsync(ChannelPost channelPost)
        {
            db.Attach(channelPost.Media);
            await db.ChannelPosts.AddAsync(channelPost);
        }

        public async Task AddPostAsync(List<ChannelPost> channelPosts)
        {
            db.AttachRange(channelPosts.Select(x => x.Media));
            await db.ChannelPosts.AddRangeAsync(channelPosts);
        }

        public async Task<Channel> ChangeType(int id, ChannelType type)
        {
            var channel = await db.Channels.FindAsync(id);
            channel.Type = type;
            Update(channel);
            await SaveAsync();
            return channel;
        }

        public void Delete(Channel channel)
        {
            db.Channels.Remove(channel);
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task<bool> Exist(long chatId)
        {
            bool result = false;
            var channel = await db.Channels.FirstOrDefaultAsync(x => x.ChatId == chatId);
            if (channel is not null) result = true;
            return result;
        }

        public async Task<Channel> FindAsync(long chatId)
        {
            return await db.Channels.FirstOrDefaultAsync(x => x.ChatId == chatId);
        }

        public async Task<Channel> FindAsync(int id)
        {
            return await db.Channels.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Channel> FindOrAddAsync(Channel channel)
        {
            var foundChannel = await FindAsync(channel.ChatId);
            if (foundChannel == null)
            {
                foundChannel = channel;
                await AddAsync(channel);
                await SaveAsync();
            }

            return foundChannel;
        }

        public Task<List<Channel>> GetOtherChannelsAsync() =>
            db.Channels.Where(x => x.Type != ChannelType.Admins).ToListAsync();

        public Task<List<Channel>> GetAdminChannelsAsync() =>
            db.Channels.Where(x => x.Type == ChannelType.Admins).ToListAsync();

        public async Task<List<Channel>> Channels()
        {
            return await db.Channels.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Update(Channel channel)
        {
            db.Channels.Update(channel);
        }
    }
}
