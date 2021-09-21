using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Interfaces
{
    public interface IUser : IDisposable
    {
        Task AddAsync(User user);
        Task SaveAsync();
        Task<User> FindAsync(Guid id);
        Task<User> FindAsync(long id);
        Task<User> FindOrAddAsync(User user);
        Task<List<User>> GetUsersAsync();
        Task<User[]> GetUsersWithAdminRequestAsync(int pageNumber);
        Task<User[]> GetAdminsAsync(int pageNumber);
        Task<List<User>> GetAllAdminsAsync();
        void Update(User user);
        void Remove(User user);
        Task<User> CheckUserAsync(long id);
        Task<User> AcceptAdminRequest(long UserId);
        Task<User> RefuseAdminRequest(long UserId);
        Task Reset(User user, params UserResetSection[] sections);
        void SendAdminRequest(User user);
        Task<User> DeleteAdmin(Guid id);
        void ChangeUserPlace(User user, UserState userPlace);
        Task<User[]> Search(string userName);
        Task<long[]> GetUserIds();
    }
}
