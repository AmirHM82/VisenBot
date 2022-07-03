using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Context;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Services
{
    public class ChannelPostService : IChannelPost
    {
        private DB db;

        public ChannelPostService(DB db)
        {
            this.db = db;
        }

        public ValueTask<EntityEntry<ChannelPost>> AddAsync(ChannelPost channelPost)
        {
            return db.ChannelPosts.AddAsync(channelPost);
        }

        public void Delete(ChannelPost channelPost)
        {
            db.ChannelPosts.Remove(channelPost);
        }

        public async Task Delete(int messageId)
        {
            ChannelPost post = await FindAsync(messageId);
            Delete(post);
            await SaveAsync();
        }

        public ValueTask<ChannelPost> FindAsync(int messageId)
        {
            return db.ChannelPosts.FindAsync(messageId);
        }

        public Task SaveAsync()
        {
            return db.SaveChangesAsync();
        }

        public void Update(ChannelPost channelPost)
        {
            db.ChannelPosts.Update(channelPost);
        }
    }
}
