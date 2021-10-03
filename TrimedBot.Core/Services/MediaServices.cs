using Dasync.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Context;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Services
{
    public class MediaServices : IMedia
    {
        protected DB _db;
        public MediaServices(DB db)
        {
            _db = db;
        }

        public async Task AddAsync(Media track)
        {
            _db.Entry(track.User).State = EntityState.Unchanged;
            await _db.Medias.AddAsync(track);
        }

        public async Task<Media> ChangeTitle(Guid Id, string Title)
        {
            Media media = await _db.Medias.FirstOrDefaultAsync(t => t.Id == Id);
            media.Title = Title;
            media.IsConfirmed = false;
            _db.Medias.Update(media);
            await _db.SaveChangesAsync();
            return media;
        }

        public async Task<Media> ChangeCaption(Guid Id, string Caption)
        {
            Media media = await _db.Medias.FirstOrDefaultAsync(t => t.Id == Id);
            media.Caption = Caption;
            media.IsConfirmed = false;
            _db.Medias.Update(media);
            await _db.SaveChangesAsync();
            return media;
        }

        public async Task<Media> ChangeVideo(Guid Id, string FileId)
        {
            Media media = await _db.Medias.FirstOrDefaultAsync(t => t.Id == Id);
            media.FileId = FileId;
            media.IsConfirmed = false;
            _db.Medias.Update(media);
            await _db.SaveChangesAsync();
            return media;
        }

        public void Confirm(Media media)
        {
            media.IsConfirmed = true;
            _db.Medias.Update(media);
        }

        public void Edit(Media track)
        {
            _db.Medias.Update(track);
        }

        public Task<Media> FindAsync(Guid id)
        {
            return _db.Medias.Include(x => x.User).Include(x => x.Tags).FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<Media> FindAsync(string Id)
        {
            return _db.Medias.Include(x => x.User).Include(x => x.Tags).FirstOrDefaultAsync(t => t.Id.ToString() == Id);
        }

        public Task<Media[]> GetMediasAsync(User user)
        {
            var m = _db.Medias.Where(x => x.User.Id == user.Id).OrderByDescending(x => x.AddDate).ToArrayAsync();
            if (m == null)
                m = _db.Medias.OrderByDescending(x => x.AddDate).ToArrayAsync();
            return m;
        }

        public Task<Media[]> GetNotConfirmedPostsAsync(int pageNumber)
        {
            return _db.Medias.Where(x => x.IsConfirmed == false).Include(x => x.User)
                    .OrderBy(x => x.AddDate).Skip(--pageNumber * 5).Take(5).ToArrayAsync();
        }

        public void Remove(Media media)
        {
            _db.Medias.Remove(media);
        }

        public async Task<Media> Remove(string Id)
        {
            var m = await _db.Medias.FirstOrDefaultAsync(t => t.Id.ToString() == Id);
            _db.Medias.Remove(m);
            return m;
        }

        public Task SaveAsync()
        {
            return _db.SaveChangesAsync();
        }

        public Task<Media[]> SearchAsync(User user, string caption)
        {
            var result = _db.Medias
                    .Where(x => x.Tags != user.BlockedTags)
                    .Where(x => x.IsConfirmed == true || x.User.Id == user.Id);

            if (caption is not null || caption != "")
                result = result.Where(x => x.Caption.Contains(caption) || x.Title.Contains(caption));

            return result.OrderByDescending(x => x.AddDate).Take(50).ToArrayAsync();
        }

        public Task<Media[]> GetMediasAsync(User user, int pageNumber)
        {
            return _db.Medias.Where(x => x.User == user)
                .OrderByDescending(x => x.AddDate).Skip((--pageNumber) * 5).Take(5).ToArrayAsync();
        }

        public Task<Media[]> GetUsersMediasAsync(Guid userId, string fileId, int pageNumber)
        {
            return _db.Medias.Include(x => x.User).Where(x => x.FileId == fileId && x.User.Id == userId)
                    .OrderByDescending(x => x.AddDate).Skip((--pageNumber) * 5).Take(5).ToArrayAsync();
        }

        public Task<Media[]> GetMediasAsync(string fileId, int pageNumber)
        {
            return _db.Medias.Include(x => x.User).Where(x => x.FileId == fileId)
                    .OrderByDescending(x => x.AddDate).Skip((--pageNumber) * 5).Take(5).ToArrayAsync();
        }

        public void Decline(Media media)
        {
            media.IsConfirmed = false;
            _db.Medias.Update(media);
        }

        public void Update(Media media)
        {
            _db.Medias.Update(media);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Task<List<Media>> GetConfirmedMedias()
        {
            return _db.Medias.Include(x => x.Tags).Where(x => x.IsConfirmed == true).ToListAsync();
        }

        public Task<Media> GetAsync(string fileId)
        {
            return _db.Medias.Where(x => x.FileId == fileId).FirstOrDefaultAsync();
        }

        public async Task RemoveTag(Guid mediaId, int tagId)
        {
            var media = await _db.Medias.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == mediaId);
            var tag = media.Tags.Where(x => x.Id == tagId);
            if (tag is null) return;
            _db.Attach(tag); //Error: The entity type 'WhereEnumerableIterator<Tag>' was not found. Ensure that the entity type has been added to the model.
            _db.Entry(tag).State = EntityState.Deleted;
            Update(media);
            await SaveAsync();
        }
    }
}
