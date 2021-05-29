using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Database.Models;

namespace TrimedBot.Core.Interfaces
{
    public interface IUser
    {
        Task AddAsync(User user);
        Task SaveAsync();
        Task<User> FindAsync(Guid id);
        Task<User> FindAsync(long id);
        Task<User> FindOrAddAsync(Telegram.Bot.Types.User user);
        Task<List<User>> GetUsersAsync();
        Task<User[]> GetUsersWithAdminRequestAsync(int pageNumber);
        Task<User[]> GetAdminsAsync(int pageNumber);
        void Update(User user);
        void Remove(User user);
        Task<User> CheckUserAsync(long id);
        Task<User> AcceptAdminRequest(long UserId);
        Task<User> RefuseAdminRequest(long UserId);
        Task Reset(User user, params UserResetSection[] sections);
        void SendAdminRequest(User user);
        Task<User> DeleteAdmin(Guid id);
        void ChangeUserPlace(User user, UserPlace userPlace);
        Task<User[]> Search(string userName);
        Task<long[]> GetUserIds();
    }
}
