﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Interfaces
{
    public interface IMedia : IDisposable
    {
        Task AddAsync(Media track);
        void Edit(Media track);
        Task<List<Media>> SearchAsync(User user, string caption);
        Task<Media[]> GetMediasAsync(User user);
        Task<List<Media>> GetMediasAsync();
        Task<Media[]> GetMediasAsync(User user, int pageNumber);
        Task<Media> FindAsync(Guid id);
        Task<Media> FindAsync(string id);
        Task<Media> GetAsync(string fileId);
        Task<Media[]> GetUsersMediasAsync(Guid userId, string FileId, int pageNumber);
        Task<Media[]> GetMediasAsync(string fileId, int pageNumber);
        Task SaveAsync();
        void Remove(Media media);
        void Update(Media media);
        Task<Media> Remove(string id);
        Task<Media> ChangeTitle(Guid Id, string Title);
        Task<Media> ChangeCaption(Guid Id, string Caption);
        Task<Media> ChangeVideo(Guid Id, string FileId);
        Task<Media[]> GetNotConfirmedPostsAsync(int pageNumber);
        Task<List<Media>> GetNotConfirmedPostsAsync();
        void Decline(Media media);
        Task Confirm(Media media);
        Task<List<Media>> GetConfirmedMedias();
        Task RemoveTag(Guid mediaId, int tagId);
        Task<int> CountAsync(Guid userId);
    }
}
