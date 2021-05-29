using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.Database.Context;
using TrimedBot.Database.Models;

namespace TrimedBot.Core.Services
{
    public class UserServices : IUser
    {
        protected DB _context;
        public UserServices(DB context)
        {
            _context = context;
        }

        public Task AddAsync(User user)
        {
            return Task.Run(async () =>
            {
                await _context.Users.AddAsync(user);
                if (user.Medias != null)
                    _context.Entry(user.Medias).State = EntityState.Unchanged;
            });
        }

        public Task<User> FindAsync(Guid id)
        {
            return Task.Run(async () =>
            {
                User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                return user;
            });
        }

        public Task<User> FindAsync(long id)
        {
            return Task.Run(async () =>
            {
                User user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
                return user;
            });
        }

        public Task<List<User>> GetUsersAsync()
        {
            return Task.Run(async () =>
            {
                List<User> users = await _context.Users.ToListAsync();
                return users;
            });
        }

        public Task SaveAsync()
        {
            return Task.Run(async () => await _context.SaveChangesAsync());
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public Task<User[]> GetUsersWithAdminRequestAsync(int pageNumber)
        {
            return Task.Run(async () =>
            {
                User[] users = await _context.Users.Where(x => x.IsSentAdminRequest == true).OrderByDescending(x => x.StartDate).Skip((--pageNumber) * 5).Take(5).ToArrayAsync();
                return users;
            });
        }

        public void Remove(User users)
        {
            _context.Users.RemoveRange(users);
        }

        public Task<User> CheckUserAsync(long id)
        {
            return Task.Run(async () =>
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
                if (user == null)
                {
                    user = new Database.Models.User
                    {
                        Id = Guid.NewGuid(),
                        UserPlace = Database.Models.UserPlace.NoWhere,
                        Access = Database.Models.Access.Member,
                        IsSentAdminRequest = false,
                        StartDate = DateTime.UtcNow,
                        UserId = id
                    };

                    await _context.Users.AddAsync(user);

                    await SaveAsync();
                }

                return user;
            });
        }

        public Task<User> AcceptAdminRequest(long UserId)
        {
            return Task.Run(async () =>
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == UserId);
                user.IsSentAdminRequest = false;
                user.Access = Access.Admin;
                _context.Users.Update(user);
                _context.SaveChanges();
                return user;
            });
        }

        public Task<User> RefuseAdminRequest(long UserId)
        {
            return Task.Run(async () =>
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == UserId);
                user.IsSentAdminRequest = false;
                _context.Users.Update(user);
                _context.SaveChanges();
                return user;
            });
        }

        public Task Reset(User user, params UserResetSection[] sections)
        {
            return Task.Run(async () =>
            {
                if (sections != null)
                    for (int i = 0; i < sections.Length; i++)
                    {
                        switch (sections[i])
                        {
                            case UserResetSection.Temp:
                                user.Temp = null;
                                break;
                            case UserResetSection.UserPlace:
                                user.UserPlace = UserPlace.NoWhere;
                                break;
                            default:
                                break;
                        }
                    }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            });
        }

        public void SendAdminRequest(User user)
        {
            user.IsSentAdminRequest = true;
            _context.Users.Update(user);
        }

        public Task<User[]> GetAdminsAsync(int pageNumber)
        {
            return Task.Run(async () =>
            {
                User[] users = await _context.Users.Where(x => x.Access == Access.Admin).OrderByDescending(x => x.StartDate).Skip(--pageNumber * 5).Take(5).ToArrayAsync();
                return users;
            });
        }

        public Task<User> DeleteAdmin(Guid id)
        {
            return Task.Run(async () =>
            {
                User user = await FindAsync(id);
                if (user == null) return null;
                Remove(user);
                await SaveAsync();
                return user;
            });
        }

        public void ChangeUserPlace(User user, UserPlace userPlace)
        {
            user.UserPlace = userPlace;
            Update(user);
        }

        public Task<User> FindOrAddAsync(Telegram.Bot.Types.User user)
        {
            return Task.Run(async () =>
            {
                var foundUser = await FindAsync(user.Id);
                if (foundUser == null)
                {
                    foundUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Access = Access.Member,
                        IsSentAdminRequest = false,
                        IsBanned = false,
                        StartDate = DateTime.UtcNow,
                        UserId = user.Id,
                        UserPlace = UserPlace.NoWhere,
                        UserName = user.Username
                    };
                    await AddAsync(foundUser);
                }

                return foundUser;
            });
        }

        public Task<User[]> Search(string userName)
        {
            return Task.Run(async () =>
            {
                var selectedUsers = await _context.Users.Where(x => x.UserName.Contains(userName)).ToArrayAsync();
                return selectedUsers;
            });
        }

        public Task<long[]> GetUserIds()
        {
            return Task.Run(async () =>
            {
                var userIds = await _context.Users.Select(x => x.UserId).ToArrayAsync();
                return userIds;
            });
        }
    }
}
