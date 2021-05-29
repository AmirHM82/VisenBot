using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Database.Models;

namespace TrimedBot.Core.Interfaces
{
    public interface IMedia
    {
        Task AddAsync(Media track);
        void Edit(Media track);
        Task<Media[]> SearchAsync(Guid Id,string caption);
        Task<Media[]> GetMediasAsync(User user);
        Task<Media[]> GetMediasAsync(User user, int pageNumber);
        Task<Media> FindAsync(Guid id);
        Task<Media> FindAsync(string id);
        Task<Media[]> GetUsersMediasAsync(Guid userId, string FileId, int pageNumber);
        Task<Media[]> GetMediasAsync(string fileId, int pageNumber);
        Task SaveAsync();
        void Remove(Media media);
        Task<Media> Remove(string id);
        Task<Media> ChangeTitle(Guid Id, string Caption);
        Task<Media> ChangeCaption(Guid Id, string Caption);
        Task<Media> ChangeVideo(Guid Id, string FileId);
        Task<Media[]> GetNotConfirmedPostsAsync(int pageNumber);
        void Decline(Media media);
        void Confirm(Media media);
    }
}
