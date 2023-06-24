using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Context;
using TrimedBot.DAL.Entities;
using TrimedBot.DAL.Enums;
using Z.EntityFramework.Plus;

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
            db.Entry(channelPost.Channel).State = EntityState.Unchanged;
            db.Entry(channelPost.Media).State = EntityState.Unchanged;
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

        public Task<ChannelPost> FindAsync(long chatdId, int messageId)
        {
            return db.ChannelPosts.FirstOrDefaultAsync(x => x.Channel.ChatId == chatdId && x.MessageId == messageId);
        }

        public async Task Delete(long chatId, int messageId)
        {
            //Do it with "entity framework plus"
            //With this package u can delete entites directly without recieving them
            var post = await FindAsync(chatId, messageId);
            if (post is null) return;
            Delete(post);
            await SaveAsync();
        }

        public async Task<List<ChannelPost>> GetAndDelete(long chatId, PostType type)
        {
            var posts = await db.ChannelPosts.Where(x => x.Channel.ChatId == chatId && x.PostType == type).ToListAsync();
            //posts.ForEach(post => db.Entry(post).State = EntityState.Deleted);
            db.RemoveRange(posts);
            await SaveAsync();
            return posts;
        }

        public Task AddAsync(List<ChannelPost> channelPosts)
        {
            return db.ChannelPosts.AddRangeAsync(channelPosts);
        }

        public async Task<List<ChannelPost>> GetAndDelete(Guid mediaId)
        {
            var posts = await db.ChannelPosts.Include(x => x.Channel).Include(x => x.Media).Where(x => x.Media.Id == mediaId).ToListAsync();

            if (!posts.Any())
                return null;

            db.ChannelPosts.RemoveRange(posts);
            await SaveAsync();

            return posts;
        }
    }
}
