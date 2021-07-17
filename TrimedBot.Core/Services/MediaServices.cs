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

        public Task AddAsync(Media track)
        {
            return Task.Run(async () =>
            {
                _db.Entry(track.User).State = EntityState.Unchanged;
                await _db.Medias.AddAsync(track);
            });
        }

        public Task<Media> ChangeTitle(Guid Id, string Title)
        {
            return Task.Run(async () =>
            {
                Media media = await _db.Medias.FirstOrDefaultAsync(t => t.Id == Id);
                media.Title = Title;
                media.IsConfirmed = false;
                _db.Medias.Update(media);
                await _db.SaveChangesAsync();
                return media;
            });
        }

        public Task<Media> ChangeCaption(Guid Id, string Caption)
        {
            return Task.Run(async () =>
            {
                Media media = await _db.Medias.FirstOrDefaultAsync(t => t.Id == Id);
                media.Caption = Caption;
                media.IsConfirmed = false;
                _db.Medias.Update(media);
                await _db.SaveChangesAsync();
                return media;
            });
        }

        public Task<Media> ChangeVideo(Guid Id, string FileId)
        {
            return Task.Run(async () =>
            {
                Media media = await _db.Medias.FirstOrDefaultAsync(t => t.Id == Id);
                media.FileId = FileId;
                media.IsConfirmed = false;
                _db.Medias.Update(media);
                await _db.SaveChangesAsync();
                return media;
            });
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
            return _db.Medias.Include(x => x.User).FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<Media> FindAsync(string Id)
        {
            return _db.Medias.Include(x => x.User).FirstOrDefaultAsync(t => t.Id.ToString() == Id);
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
                    .OrderByDescending(x => x.AddDate).Skip(--pageNumber * 5).Take(5).ToArrayAsync();
        }

        public void Remove(Media media)
        {
            _db.Medias.Remove(media);
        }

        public Task<Media> Remove(string Id)
        {
            return Task.Run(async () =>
            {
                var m = await _db.Medias.FirstOrDefaultAsync(t => t.Id.ToString() == Id);
                _db.Medias.Remove(m);
                return m;
            });
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public Task<Media[]> SearchAsync(Guid userId, string caption)
        {
                return _db.Medias.Where(x => (x.Caption.Contains(caption) || x.Title.Contains(caption)) && (x.IsConfirmed == true || x.User.Id == userId))
                    .OrderByDescending(x => x.AddDate).ToArrayAsync();
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
    }
}
