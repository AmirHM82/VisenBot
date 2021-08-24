using Microsoft.EntityFrameworkCore;
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
    public class TagService : ITag
    {
        protected DB db;

        public TagService(DB db)
        {
            this.db = db;
        }

        public async Task AddAsync(Tag tag)
        {
            await db.Tags.AddAsync(tag);
        }

        public void Delete(Tag tag)
        {
            db.Tags.Remove(tag);
        }

        public async Task Delete(int tagId)
        {
            db.Tags.Remove(await FindAsync(tagId));
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task<Tag> FindAsync(int tagId)
        {
            return await db.Tags.FirstOrDefaultAsync(x => x.Id == tagId);
        }

        public async Task<Tag> FindIncludeMediaAsync(int tagId)
        {
            return await db.Tags.Include(x => x.Medias).FirstOrDefaultAsync(x => x.Id == tagId);
        }

        public async Task<List<Tag>> GetTagsAsync(int pageNum)
        {
            return await db.Tags.Skip((pageNum - 1) * 5).Take(5).ToListAsync();
        }

        public async Task<List<Tag>> GetTagsAsync(Guid postId, int pageNum)
        {
            return await db.Tags.Include(x => x.Medias)/*.Where(x => x.Medias.Id == postId)*/
                .Skip((pageNum - 1) * 5).Take(5).ToListAsync();
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public async Task<List<Tag>> Search(string name)
        {
            return await db.Tags.Where(x => x.Name.Contains(name)).ToListAsync();
        }

        public async Task<List<Tag>> Search(Guid postId, string name)
        {
            return await db.Tags.Include(x => x.Medias).Where(x => x.Name.Contains(name)/* && x.Medias.Id == postId*/).ToListAsync();
        }

        public void Update(Tag tag)
        {
            db.Tags.Update(tag);
        }
    }
}
