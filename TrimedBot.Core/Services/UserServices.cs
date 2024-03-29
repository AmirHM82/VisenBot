﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.Core.Interfaces;
using TrimedBot.DAL.Context;
using TrimedBot.DAL.Enums;
using TrimedBot.DAL.Entities;

namespace TrimedBot.Core.Services
{
    public class UserServices : IUser
    {
        protected DB _context;
        public UserServices(DB context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            //if (user.Medias != null)
            //    _context.Entry(user.Medias).State = EntityState.Unchanged;
        }

        public Task<User> FindAsync(Guid id)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<User> FindAsync(long id)
        {
            return _context.Users.Include(x => x.BlockedTags).FirstOrDefaultAsync(x => x.UserId == id);
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

        public async Task<User> CheckUserAsync(long id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    UserState = UserState.NoWhere,
                    Access = Access.Member,
                    IsSentAdminRequest = false,
                    StartDate = DateTime.UtcNow,
                    UserId = id
                };

                await _context.Users.AddAsync(user);

                await SaveAsync();
            }

            return user;
        }

        public async Task<User> AcceptAdminRequest(long UserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == UserId);
            user.IsSentAdminRequest = false;
            user.Access = Access.Admin;
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }

        public async Task<User> RefuseAdminRequest(long UserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == UserId);
            user.IsSentAdminRequest = false;
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }

        public async Task Reset(User user, params UserResetSection[] sections)
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
                            user.UserState = UserState.NoWhere;
                            break;
                        default:
                            break;
                    }
                }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public void SendAdminRequest(User user)
        {
            user.IsSentAdminRequest = true;
            _context.Users.Update(user);
        }

        public Task<User[]> GetAdminsAsync(int pageNumber)
        {
            return _context.Users.Where(x => x.Access == Access.Admin)
                .OrderByDescending(x => x.StartDate).Skip(--pageNumber * 5).Take(5).ToArrayAsync();
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

        public void ChangeUserPlace(User user, UserState userPlace)
        {
            user.UserState = userPlace;
            Update(user);
        }

        public async Task<User> FindOrAddAsync(User user)
        {
            var foundUser = await FindAsync(user.UserId);
            if (foundUser == null)
            {
                foundUser = user;
                await AddAsync(user);
                await SaveAsync();
            }

            return foundUser;
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

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<List<User>> GetAllAdminsAsync()
        {
            return await _context.Users.Where(x => x.Access == Access.Admin).ToListAsync();
        }
    }
}
